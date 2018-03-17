本总结主要针对于WPF嵌入Xilium.CefGlue
Xilium.CefGlue WPF嵌入.源码修改:

1-关于CEF加载路径问题.
修改:Load(null);
为Load(Directory.GetCurrentDirectory()+"\\cef\\");Directory.GetCurrentDirectory()+"\\cef\\"
可以为自己定义的目录.

2-HASH API错误.修改编译设置平台为x86.

2-屏蔽超级链接弹窗 file:CefLifeSpanHandler.cs
修改代码:
 protected virtual bool OnBeforePopup(CefBrowser browser, CefFrame frame, string targetUrl, string targetFrameName, CefWindowOpenDisposition targetDisposition, bool userGesture, CefPopupFeatures popupFeatures, CefWindowInfo windowInfo, ref CefClient client, CefBrowserSettings settings, ref bool noJavascriptAccess)
        {
           
            return false;
        }
为:
 protected virtual bool OnBeforePopup(CefBrowser browser, CefFrame frame, string targetUrl, string targetFrameName, CefWindowOpenDisposition targetDisposition, bool userGesture, CefPopupFeatures popupFeatures, CefWindowInfo windowInfo, ref CefClient client, CefBrowserSettings settings, ref bool noJavascriptAccess)
        {
           
            return true;
        }
修改完毕后仅仅不再弹出弹窗.但是主browser并不加载超级链接地址


因为CEF3关闭沙盒后。首次加载flash会有一个CMD弹窗。用户体验极度不好。
本教程主要修改flash文件来实现屏蔽弹窗功能。
找到pepflashplayer.dll文件。采用16进制编辑文件
本人推荐WinHex工具打开以上面dll文件
然后按Ctrl+F搜索
搜索设置：区分大小写√去掉。采用ASCII搜索
搜索字符串：COMSPEC 会显示COMSPEC.cmd.exe
把COMSPEC.cmd.exe修改为CO1SPEC.cm1.exe即可
这里不是一定非要改成1.把他改成其他的都可以。反正就是不要让他愉快调用CMD即可
同时这里存在一个漏洞。既然可以调用cmd那么能不能注入调用别的。
所以这个东西都有两面性。剩下的请自行脑洞