using System;

namespace Xilium.CefGlue.WPF
{
    internal sealed class WpfCefLifeSpanHandler : CefLifeSpanHandler
    {
        private readonly WpfCefBrowser _owner;

        public WpfCefLifeSpanHandler(WpfCefBrowser owner)
        {
            if (owner == null) throw new ArgumentNullException("owner");

            _owner = owner;
        }

        protected override void OnAfterCreated(CefBrowser browser)
        {
            _owner.HandleAfterCreated(browser);
        }
        //以下代码是允许一个窗体打开所有连接
        protected override bool DoClose(CefBrowser browser)
        {
            return false;
        }
        protected override bool OnBeforePopup(CefBrowser browser, CefFrame frame, string targetUrl, string targetFrameName, CefWindowOpenDisposition targetDisposition, bool userGesture, CefPopupFeatures popupFeatures, CefWindowInfo windowInfo, ref CefClient client, CefBrowserSettings settings, ref bool noJavascriptAccess)
        {
            bool res = false;
            if (!string.IsNullOrEmpty(targetUrl))
            {
                CefRequest req = CefRequest.Create();
                req.FirstPartyForCookies = null; //webBrowser.selfRequest.FirstPartyForCookies;
                req.Options = CefUrlRequestOptions.AllowCachedCredentials;//webBrowser.selfRequest.Options;
                System.Collections.Specialized.NameValueCollection h = new System.Collections.Specialized.NameValueCollection();
                h.Add("Content-Type", "application/x-www-form-urlencoded");
                req.Set(targetUrl, null, null, h);
                //_owner. = req;
                //在该处实现用当前页面打开所有新开的窗口  
                _owner.NavigateTo(targetUrl);
                res = true;
                if (res)
                    return res;
            }
            return base.OnBeforePopup(browser, frame, targetUrl, targetFrameName, targetDisposition, userGesture, popupFeatures, windowInfo, ref client, settings, ref noJavascriptAccess);

        }
    }
}
