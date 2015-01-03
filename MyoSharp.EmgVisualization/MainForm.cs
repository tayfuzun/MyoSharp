using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ZedGraph;

using MyoSharp.Communication;
using MyoSharp.Device;

namespace MyoSharp.EmgVisualization
{
    /// <summary>
    /// The main form of the application.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Constants
        private const int NUMBER_OF_SENSORS = 8;

        private static readonly Color[] DATA_SERIES_COLORS = new Color[]
        {
            Color.Red,
            Color.Blue,
            Color.Green,
            Color.Yellow,
            Color.Pink,
            Color.Orange,
            Color.Purple,
            Color.Black,
        };
        #endregion

        #region Fields
        private readonly DateTime _startTime;
        private readonly PointPairList[] _pointPairs;
        private readonly ZedGraphControl _graphControl;
        private readonly IChannel _channel;
        private readonly IHub _hub;
        private readonly List<LineItem> _sortOrderZ;
        #endregion
        
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            // we'll calculate all of our incoming data relative to this point in time
            _startTime = DateTime.UtcNow;

            // construct our graph
            _graphControl = new ZedGraphControl()
            {
                Dock = DockStyle.Fill,
            };

            _graphControl.MouseClick += GraphControl_MouseClick;
            _graphControl.GraphPane.Title.Text = "Myo EMG Data vs Time";
            _graphControl.GraphPane.YAxis.Title.Text = "EMG Value";
            _graphControl.GraphPane.XAxis.Title.Text = "Time (Seconds)";

            _pointPairs = new PointPairList[NUMBER_OF_SENSORS];
            _sortOrderZ = new List<LineItem>();
            for (var i = 0; i < _pointPairs.Length; ++i)
            {
                _pointPairs[i] = new PointPairList();

                var dataPointLine = _graphControl.GraphPane.AddCurve("Sensor " + i, _pointPairs[i], Color.Black);
                dataPointLine.Line.IsVisible = false;
                dataPointLine.Symbol.Fill = new Fill(DATA_SERIES_COLORS[i]);

                _sortOrderZ.Add(dataPointLine);
            }

            Controls.Add(_graphControl);

            // get set up to listen for Myo events
            _channel = Channel.Create(ChannelDriver.Create(ChannelBridge.Create()));
            
            _hub = Hub.Create(_channel);
            _hub.MyoConnected += Hub_MyoConnected;
            _hub.MyoDisconnected += Hub_MyoDisconnected;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // start listening for Myo data
            _channel.StartListening();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closed" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnClosed(EventArgs e)
        {
            _channel.Dispose();
            _hub.Dispose();

            base.OnClosed(e);
        }

        private void RefreshGraph()
        {
            // force a redraw for new data
            _graphControl.AxisChange();
            _graphControl.Invalidate();
        }

        private void SortZOrderFromClickLocation(ZedGraphControl graphControl, float locationX, float locationY)
        {
            object nearestObject;
            int index;
            graphControl.GraphPane.FindNearestObject(
                new PointF(locationX, locationY),
                CreateGraphics(),
                out nearestObject,
                out index);

            if (nearestObject == null)
            {
                return;
            }

            LineItem activeLine = null;
            if (nearestObject.GetType() == typeof(LineItem))
            {
                activeLine = (LineItem)nearestObject;
            }
            else if (nearestObject.GetType() == typeof(Legend))
            {
                var legend = (Legend)nearestObject;

                legend.FindPoint(
                    new PointF(locationX, locationY),
                    graphControl.GraphPane,
                    graphControl.GraphPane.CalcScaleFactor(),
                    out index);

                if (index >= 0 && index < _sortOrderZ.Count)
                {
                    activeLine = _sortOrderZ[index];
                }
            }

            if (activeLine != null)
            {
                _sortOrderZ.Remove(activeLine);
                _sortOrderZ.Insert(0, activeLine);

                graphControl.GraphPane.CurveList.Sort(new CurveItemComparer(_sortOrderZ));
                graphControl.Invalidate();
            }
        }
        #endregion

        #region Event Handlers
        private void Hub_MyoDisconnected(object sender, MyoEventArgs e)
        {
            e.Myo.EmgDataAcquired -= Myo_EmgDataAcquired;
        }

        private void Hub_MyoConnected(object sender, MyoEventArgs e)
        {
            e.Myo.EmgDataAcquired += Myo_EmgDataAcquired;
            e.Myo.SetEmgStreaming(true);
        }

        private void Myo_EmgDataAcquired(object sender, EmgDataEventArgs e)
        {
            // pull data from each sensor
            for (var i = 0; i < _pointPairs.Length; ++i)
            {
                _pointPairs[i].Add((e.Timestamp - _startTime).TotalSeconds, e.EmgData.GetDataForSensor(i));
            }
        }

        private void TmrRefresh_Tick(object sender, EventArgs e)
        {
            // timer UI component ticks on the UI thread!
            RefreshGraph();
        }

        private void GraphControl_MouseClick(object sender, MouseEventArgs e)
        {
            var graphControl = (ZedGraphControl)sender;
            SortZOrderFromClickLocation(graphControl, e.X, e.Y);
        }
        #endregion

        #region Classes
        private class CurveItemComparer : IComparer<CurveItem>
        {
            private readonly IList<CurveItem> _sortOrder;

            public CurveItemComparer(IEnumerable<CurveItem> sortOrder)
            {
                _sortOrder = new List<CurveItem>(sortOrder);
            }

            public int Compare(CurveItem x, CurveItem y)
            {
                return _sortOrder.IndexOf(x).CompareTo(_sortOrder.IndexOf(y));
            }
        }
        #endregion
    }
}
