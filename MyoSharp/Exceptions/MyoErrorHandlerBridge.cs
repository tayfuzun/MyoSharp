using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Text;

using MyoSharp.Communication;
using MyoSharp.Exceptions;
using MyoSharp.Internal;

namespace MyoSharp.Exceptions
{
    public class MyoErrorHandlerBridge : IMyoErrorHandlerBridge
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MyoErrorHandlerBridge"/> class.
        /// </summary>
        /// <param name="I">The i.</param>
        private MyoErrorHandlerBridge()
        {
        }
        #endregion

        #region Methods
        public static IMyoErrorHandlerBridge Create()
        {
            Contract.Ensures(Contract.Result<IMyoErrorHandlerBridge>() != null);

            return new MyoErrorHandlerBridge();
        }
        
        /// <inheritdoc />
        public string LibmyoErrorCstring32(IntPtr errorHandle)
        {
            var result = libmyo_error_cstring_32(errorHandle);
            return result ?? string.Empty;
        }

        /// <inheritdoc />
        public string LibmyoErrorCstring64(IntPtr errorHandle)
        {
            var result = libmyo_error_cstring_64(errorHandle);
            return result ?? string.Empty;
        }

        /// <inheritdoc />
        public void LibmyoFreeErrorDetails32(IntPtr errorHandle)
        {
            libmyo_free_error_details_32(errorHandle);
        }

        /// <inheritdoc />
        public void LibmyoFreeErrorDetails64(IntPtr errorHandle)
        {
            libmyo_free_error_details_64(errorHandle);
        }
        #endregion

        #region PInvokes
        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_error_cstring", CallingConvention = CallingConvention.Cdecl)]
        private static extern string libmyo_error_cstring_32(IntPtr errorHandle);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_error_cstring", CallingConvention = CallingConvention.Cdecl)]
        private static extern string libmyo_error_cstring_64(IntPtr errorHandle);

        [DllImport(PlatformInvocation.MyoDllPath32, EntryPoint = "libmyo_free_error_details", CallingConvention = CallingConvention.Cdecl)]
        private static extern void libmyo_free_error_details_32(IntPtr errorHandle);

        [DllImport(PlatformInvocation.MyoDllPath64, EntryPoint = "libmyo_free_error_details", CallingConvention = CallingConvention.Cdecl)]
        private static extern void libmyo_free_error_details_64(IntPtr errorHandle);
        #endregion
    }
}
