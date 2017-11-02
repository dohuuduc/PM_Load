using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Text;

namespace Web
{
    class WebToolkit
    {

        private static CookieContainer cookieContainer = new CookieContainer();

        private static string UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";

        public static string Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        public static string ContentType = "application/x-www-form-urlencoded";
        public static string Method = "GET";

        public static string Referer = "";

        public static bool IncludeCookies = true;

        public static void SetDefaultInfo()
        {
            Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            ContentType = "application/x-www-form-urlencoded";
            Method = "GET";

            Referer = "";

            IncludeCookies = true;
        }

        public static void SetInfo(HttpWebRequest httpWebRequest, string data = null, WebHeaderCollection headerCollection = null)
        {
            List<string> listUploadContentType = new List<string>();
            listUploadContentType.Add("image/png");
            listUploadContentType.Add("mage/jpg");

            if (headerCollection != null)
            {
                httpWebRequest.Headers = headerCollection;
            }

            httpWebRequest.ContentType = ContentType;
            httpWebRequest.UserAgent = UserAgent;
            httpWebRequest.Accept = Accept;
            httpWebRequest.Method = Method;

            //httpWebRequest.Timeout = 5000 * 1000;

            httpWebRequest.Referer = httpWebRequest.Host;

            if (IncludeCookies)
            {
                httpWebRequest.CookieContainer = cookieContainer;
            } 

            httpWebRequest.KeepAlive = true;
            httpWebRequest.AllowAutoRedirect = false;
            httpWebRequest.ServicePoint.Expect100Continue = false;

            httpWebRequest.ProtocolVersion = HttpVersion.Version11;

            // ServicePointManager.Expect100Continue = false;

            if (Referer != "")
            {
                httpWebRequest.Referer = Referer;
            }

            if (data != null)
            {
                //data = Uri.EscapeUriString(data);

                httpWebRequest.Method = "POST";
                if (Method != "GET")
                {
                    httpWebRequest.Method = Method;
                }

                if (listUploadContentType.Contains(ContentType))
                {
                    byte[] byteData = File.ReadAllBytes(data);

                    Stream requestStream = httpWebRequest.GetRequestStream();
                    requestStream.Write(byteData, 0, byteData.Length);
                    requestStream.Close();
                }
                else
                {
                    StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                    streamWriter.Write(data);
                    streamWriter.Close();
                }
            }
        }

        public static string GetCookie(string name, string url)
        {
            string value = "";

            foreach (Cookie cookie in cookieContainer.GetCookies(new Uri(url)))
            {
                if (cookie.Name == name)
                {
                    value = cookie.Value;
                }
            }
            return value;
        }
        public static void ExportCookie(string filePath)
        {
            List<string> listCookie = new List<string>();
            FieldInfo domainTableField = cookieContainer.GetType().GetField("m_domainTable", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            Hashtable domainTable = (Hashtable)domainTableField.GetValue(cookieContainer);

            foreach (DictionaryEntry element in domainTable)
            {
                SortedList sortedList = (SortedList)element.Value.GetType().GetField("m_list", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(element.Value);
                foreach (var e in sortedList)
                {
                    CookieCollection cookieCollection = (CookieCollection)((DictionaryEntry)e).Value;
                    foreach (Cookie cookie in cookieCollection)
                    {
                        string line = cookie.Domain + "|" + cookie.Name + "|" + cookie.Value;
                        listCookie.Add(line);
                    }
                }
            }

            File.WriteAllLines(filePath, listCookie);
        }

        public static void ImportCookie(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] data = line.Split('|');

                Cookie cookie = new Cookie();
                cookie.Domain = data[0];
                cookie.Name = data[1];
                cookie.Value = data[2];

                cookieContainer.Add(cookie);
            }
        }

        public static void ClearCookie()
        {
            cookieContainer = new CookieContainer();
        }

        public static HttpWebResponse GetHttpWebResponse(string url, string data = null, WebHeaderCollection headerCollection = null)
        {
            if (url != null & url != "")
            {
                url = Uri.EscapeUriString(url);

                //Not use Uri.EscapeUriString
                //http://www.gravatar.com/avatar/f7045defcd907501a3e569cde6261023?d=http%3A%2F%2Flabs.ine.com%2Fimg%2Fine-icon.jpg

                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);

                SetInfo(httpWebRequest, data, headerCollection);

                HttpWebResponse httpWebResponse = null;

                try
                {
                    httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                }
                catch (WebException ex)
                {
                    httpWebResponse = (HttpWebResponse)ex.Response;
                    //throw;
                }

                if (httpWebResponse != null)
                {
                    if (httpWebResponse.Cookies != null)
                    {
                        cookieContainer.Add(httpWebResponse.Cookies);
                    }
                }

                SetDefaultInfo();

                return httpWebResponse;
            }
            else
            {
                throw new Exception("Wrong url!");
            }
        }

        public static Stream GetStream(string url, string data = null, WebHeaderCollection headerCollection = null)
        {
            HttpWebResponse httpWebResponse = GetHttpWebResponse(url, data, headerCollection);
            if (httpWebResponse == null) return null;
            return httpWebResponse.GetResponseStream();
        }
        public static StreamReader GetStreamReader(string url, string data = null, WebHeaderCollection headerCollection = null)
        {
            Stream responseStream = GetStream(url, data, headerCollection);
            if (responseStream == null) return null;
            StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
            return streamReader;
        }

        public static string GetHtml(string url, string data = null, WebHeaderCollection headerCollection = null)
        {
            StreamReader streamReader = GetStreamReader(url, data, headerCollection);
            if (streamReader == null) return null;
            string html = streamReader.ReadToEnd();
            streamReader.Close();
            return html;
        }
        public static string GetLocation(string url, string data = null, WebHeaderCollection headerCollection = null)
        {
            HttpWebResponse httpWebResponse = GetHttpWebResponse(url, data, headerCollection);
            if (httpWebResponse == null) return null;
            string location = httpWebResponse.GetResponseHeader("Location");
            httpWebResponse.Close();

            return location;
        }
        public static string GetFileName(string url, string data = null, WebHeaderCollection headerCollection = null)
        {
            HttpWebResponse httpWebResponse = GetHttpWebResponse(url, data, headerCollection);
            if (httpWebResponse == null) return null;
            ContentDisposition contentDisposition = new ContentDisposition(httpWebResponse.GetResponseHeader("Content-Disposition"));
            string fileName = contentDisposition.FileName;
            httpWebResponse.Close();

            return fileName;
        }
        public static string GetResponseUri(string url, string data = null, WebHeaderCollection headerCollection = null)
        {
            HttpWebResponse httpWebResponse = GetHttpWebResponse(url, data, headerCollection);
            if (httpWebResponse == null) return null;
            string uri = httpWebResponse.ResponseUri.ToString();
            httpWebResponse.Close();

            return uri;
        }

        public static long GetContentLength(string url, string data = null, WebHeaderCollection headerCollection = null)
        {
            HttpWebResponse httpWebResponse = GetHttpWebResponse(url, data, headerCollection);
            if (httpWebResponse == null) return 0;
            long contentLength = httpWebResponse.ContentLength;
            httpWebResponse.Close();

            return contentLength;
        }
        public static bool IsDirectLink(string url, string data = null, WebHeaderCollection headerCollection = null)
        {
            HttpWebResponse httpWebResponse = GetHttpWebResponse(url, data, headerCollection);
            if (httpWebResponse == null) return false;
            HttpStatusCode statusCode = httpWebResponse.StatusCode;

            List<HttpStatusCode> listCode = new List<HttpStatusCode>();
            listCode.Add(HttpStatusCode.Ambiguous);
            listCode.Add(HttpStatusCode.MultipleChoices);
            listCode.Add(HttpStatusCode.Moved);
            listCode.Add(HttpStatusCode.MovedPermanently);
            listCode.Add(HttpStatusCode.Found);
            listCode.Add(HttpStatusCode.Redirect);
            listCode.Add(HttpStatusCode.RedirectMethod);
            listCode.Add(HttpStatusCode.SeeOther);
            listCode.Add(HttpStatusCode.RedirectKeepVerb);
            listCode.Add(HttpStatusCode.TemporaryRedirect);

            Boolean result = true;

            if (listCode.Contains(statusCode))
            {
                result = false;
            }

            httpWebResponse.Close();
            return result;
        }
        public static string GetDirectLink(string url, string data = null, WebHeaderCollection headerCollection = null)
        {
            HttpWebResponse httpWebResponse = GetHttpWebResponse(url, data, headerCollection);
            if (httpWebResponse == null) return null;
            HttpStatusCode statusCode = httpWebResponse.StatusCode;
            string location = httpWebResponse.GetResponseHeader("Location");
            httpWebResponse.Close();

            List<HttpStatusCode> listCode = new List<HttpStatusCode>();
            listCode.Add(HttpStatusCode.Ambiguous);
            listCode.Add(HttpStatusCode.MultipleChoices);
            listCode.Add(HttpStatusCode.Moved);
            listCode.Add(HttpStatusCode.MovedPermanently);
            listCode.Add(HttpStatusCode.Found);
            listCode.Add(HttpStatusCode.Redirect);
            listCode.Add(HttpStatusCode.RedirectMethod);
            listCode.Add(HttpStatusCode.SeeOther);
            listCode.Add(HttpStatusCode.RedirectKeepVerb);
            listCode.Add(HttpStatusCode.TemporaryRedirect);

            string result = url;

            if (listCode.Contains(statusCode))
            {
                result = GetDirectLink(location);
            }

            return result;
        }

        

    }


    public static class MyExtensions
    {
        /*By luulong: 11/2017*/
        //https://stackoverflow.com/questions/13771083/html-agility-pack-get-all-elements-by-class
        public static bool HasClass(this HtmlAgilityPack.HtmlNode element, String className)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (String.IsNullOrWhiteSpace(className)) throw new ArgumentNullException(nameof(className));
            if (element.NodeType != HtmlAgilityPack.HtmlNodeType.Element) return false;

            HtmlAgilityPack.HtmlAttribute classAttrib = element.Attributes["class"];
            if (classAttrib == null) return false;

            Boolean hasClass = CheapClassListContains(classAttrib.Value, className, StringComparison.Ordinal);
            return hasClass;
        }

        /// <summary>Performs optionally-whitespace-padded string search without new string allocations.</summary>
        /// <remarks>A regex might also work, but constructing a new regex every time this method is called would be expensive.</remarks>
        private static Boolean CheapClassListContains(String haystack, String needle, StringComparison comparison)
        {
            if (String.Equals(haystack, needle, comparison)) return true;
            Int32 idx = 0;
            while (idx + needle.Length <= haystack.Length)
            {
                idx = haystack.IndexOf(needle, idx, comparison);
                if (idx == -1) return false;

                Int32 end = idx + needle.Length;

                // Needle must be enclosed in whitespace or be at the start/end of string
                Boolean validStart = idx == 0 || Char.IsWhiteSpace(haystack[idx - 1]);
                Boolean validEnd = end == haystack.Length || Char.IsWhiteSpace(haystack[end]);
                if (validStart && validEnd) return true;

                idx++;
            }
            return false;
        }
    }
}
