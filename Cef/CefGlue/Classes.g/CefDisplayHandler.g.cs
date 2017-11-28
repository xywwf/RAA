//
// DO NOT MODIFY! THIS IS AUTOGENERATED FILE!
//
namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;
    
    // Role: HANDLER
    public abstract unsafe partial class CefDisplayHandler
    {
        private static Dictionary<IntPtr, CefDisplayHandler> _roots = new Dictionary<IntPtr, CefDisplayHandler>();
        
        private int _refct;
        private cef_display_handler_t* _self;
        
        protected object SyncRoot { get { return this; } }
        
        private cef_display_handler_t.add_ref_delegate _ds0;
        private cef_display_handler_t.release_delegate _ds1;
        private cef_display_handler_t.has_one_ref_delegate _ds2;
        private cef_display_handler_t.on_address_change_delegate _ds3;
        private cef_display_handler_t.on_title_change_delegate _ds4;
        private cef_display_handler_t.on_favicon_urlchange_delegate _ds5;
        private cef_display_handler_t.on_fullscreen_mode_change_delegate _ds6;
        private cef_display_handler_t.on_tooltip_delegate _ds7;
        private cef_display_handler_t.on_status_message_delegate _ds8;
        private cef_display_handler_t.on_console_message_delegate _ds9;
        private cef_display_handler_t.on_auto_resize_delegate _dsa;
        
        protected CefDisplayHandler()
        {
            _self = cef_display_handler_t.Alloc();
        
            _ds0 = new cef_display_handler_t.add_ref_delegate(add_ref);
            _self->_base._add_ref = Marshal.GetFunctionPointerForDelegate(_ds0);
            _ds1 = new cef_display_handler_t.release_delegate(release);
            _self->_base._release = Marshal.GetFunctionPointerForDelegate(_ds1);
            _ds2 = new cef_display_handler_t.has_one_ref_delegate(has_one_ref);
            _self->_base._has_one_ref = Marshal.GetFunctionPointerForDelegate(_ds2);
            _ds3 = new cef_display_handler_t.on_address_change_delegate(on_address_change);
            _self->_on_address_change = Marshal.GetFunctionPointerForDelegate(_ds3);
            _ds4 = new cef_display_handler_t.on_title_change_delegate(on_title_change);
            _self->_on_title_change = Marshal.GetFunctionPointerForDelegate(_ds4);
            _ds5 = new cef_display_handler_t.on_favicon_urlchange_delegate(on_favicon_urlchange);
            _self->_on_favicon_urlchange = Marshal.GetFunctionPointerForDelegate(_ds5);
            _ds6 = new cef_display_handler_t.on_fullscreen_mode_change_delegate(on_fullscreen_mode_change);
            _self->_on_fullscreen_mode_change = Marshal.GetFunctionPointerForDelegate(_ds6);
            _ds7 = new cef_display_handler_t.on_tooltip_delegate(on_tooltip);
            _self->_on_tooltip = Marshal.GetFunctionPointerForDelegate(_ds7);
            _ds8 = new cef_display_handler_t.on_status_message_delegate(on_status_message);
            _self->_on_status_message = Marshal.GetFunctionPointerForDelegate(_ds8);
            _ds9 = new cef_display_handler_t.on_console_message_delegate(on_console_message);
            _self->_on_console_message = Marshal.GetFunctionPointerForDelegate(_ds9);
            _dsa = new cef_display_handler_t.on_auto_resize_delegate(on_auto_resize);
            _self->_on_auto_resize = Marshal.GetFunctionPointerForDelegate(_dsa);
        }
        
        ~CefDisplayHandler()
        {
            Dispose(false);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (_self != null)
            {
                cef_display_handler_t.Free(_self);
                _self = null;
            }
        }
        
        private void add_ref(cef_display_handler_t* self)
        {
            lock (SyncRoot)
            {
                var result = ++_refct;
                if (result == 1)
                {
                    lock (_roots) { _roots.Add((IntPtr)_self, this); }
                }
            }
        }
        
        private int release(cef_display_handler_t* self)
        {
            lock (SyncRoot)
            {
                var result = --_refct;
                if (result == 0)
                {
                    lock (_roots) { _roots.Remove((IntPtr)_self); }
                    return 1;
                }
                return 0;
            }
        }
        
        private int has_one_ref(cef_display_handler_t* self)
        {
            lock (SyncRoot) { return _refct == 1 ? 1 : 0; }
        }
        
        internal cef_display_handler_t* ToNative()
        {
            add_ref(_self);
            return _self;
        }
        
        [Conditional("DEBUG")]
        private void CheckSelf(cef_display_handler_t* self)
        {
            if (_self != self) throw ExceptionBuilder.InvalidSelfReference();
        }
        
    }
}
