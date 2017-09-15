using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Load
{
    public class MyDownloader
    {
        WebBrowser _wb;
        TaskCompletionSource<string> _tcs;
        ApplicationContext _ctx;

        public void setWebbrowser(WebBrowser _wb) {
            this._wb = _wb;
        }
        public Task<string> Download(string url)
        {
            _tcs = new TaskCompletionSource<string>();

            var t = new Thread(() =>
            {
                // _wb = new WebBrowser();
                _wb.ScriptErrorsSuppressed = true;
                _wb.DocumentCompleted += _wb_DocumentCompleted;
                _wb.Navigate(url);
                _ctx = new ApplicationContext();
                Application.Run(_ctx);
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            return _tcs.Task;
        }

        void _wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (_wb.ReadyState != WebBrowserReadyState.Complete)
                return;

            //_tcs.TrySetResult(_wb.DocumentText);
            _tcs.TrySetResult(_wb.Document.Body.OuterHtml);
            _ctx.ExitThread();
          
        }
    }



   
}
