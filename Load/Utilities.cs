using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StorePhone;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using COMExcel = Microsoft.Office.Interop.Excel;
using System.Web;
using HtmlAgilityPack;
using Web;

namespace Load {
  public class RowAdder {
    #region Data
    private readonly object mLockObject = new object();
    private static RowAdder mInstance;

    public static RowAdder Instance {
      get {
        if (mInstance == null) {
          mInstance = new RowAdder();
        }
        return mInstance;
      }
    }

    object mSync;
    #endregion

    #region Constructor
    private RowAdder() {
    }
    #endregion

    public void Add(DataTable table, DataRow row, ref object arrControl) {
      lock (mLockObject) {
        ArrayList arr1 = (ArrayList)arrControl;
        Label lbl_message = (Label)arr1[0];
        Label lbl_moi = (Label)arr1[2];

        table.Rows.Add(row);
        lbl_moi.Text = string.Format("II-Danh Sách Liên Kết Mới: {0}", table.Rows.Count);
      }
    }
    public void Remove(ref DataTable table, int id, ref LinkQueues queue, ref object arrControl) {
      try {
        lock (mLockObject) {
          ArrayList arr1 = (ArrayList)arrControl;
          Label lbl_message = (Label)arr1[0];
          Label lbl_moi = (Label)arr1[2];
          Label lbl_scanner_chuaquet = (Label)arr1[3];
          DataGridView gv_scanner_chon = (DataGridView)arr1[4];

          if (table.Select(string.Format("id_chon={0}", id)).Count() != 0)
            table.Select(string.Format("id_chon={0}", id))[0].Delete();

          lbl_scanner_chuaquet.Text = string.Format("Danh Sách Liên Kết Chưa Quét: {0}", queue.CountQueue1());
          lbl_scanner_chuaquet.Update();
          gv_scanner_chon.Update();
        }
      }
      catch {

      }

    }
  }

  class Utilities { }

  #region Write LinkQueues
  public class LinkQueues {
    private static LinkQueues instance;
    private static Queue<dm_scanner_ct> LinksQueue1;
    private static int daxuly = 0;

    private LinkQueues() { }

    public static LinkQueues Instance {
      get {
        // If the instance is null then create one and init the Queue
        if (instance == null) {
          instance = new LinkQueues();
          LinksQueue1 = new Queue<dm_scanner_ct>();
        }
        return instance;
      }
    }

    public int CountQueue1() {
      return LinksQueue1.Count();
    }
    public int DaXuLy() {
      return daxuly;
    }

    public void EnqueueLinks1(dm_scanner_ct link) {
      lock (LinksQueue1) {
        if (LinksQueue1.Count(x => x.id == link.id) == 0)
          LinksQueue1.Enqueue(link);
      }
    }

    /// <summary>
    /// Flushes the Queue to the physical log file
    /// </summary>
    public dm_scanner_ct DequeueLinks1() {
      if (LinksQueue1.Any()) {
        dm_scanner_ct entry = new dm_scanner_ct();
        entry = LinksQueue1.Dequeue();
        daxuly++;
        entry.statur = true;
        SQLDatabase.Updatedm_scanner_ct(entry);
        return entry;
      }
      else
        return null;

    }

  }
  #endregion


  #region Write LinkQueues
  public class LinkQueues2 {
    private static LinkQueues2 instance;
    //private static List<dm_scanner_ct> LinksAll;
    private static Queue<dm_scanner_ct> LinksQueue1;

    private static int maxLogAge = int.Parse("5000");
    private static int queueSize = int.Parse("0");

    private static int daxuly = 0;
    private static int _id = 0;


    public void setIdLienKet(int id) {
      _id = id;
    }
    public int getIdLienKet() {
      return _id;
    }

    /// <summary>
    /// An LogWriter instance that exposes a single instance
    /// </summary>
    public static LinkQueues2 Instance {
      get {
        // If the instance is null then create one and init the Queue
        if (instance == null) {
          instance = new LinkQueues2();
          //LinksAll = new List<dm_scanner_ct>();
          LinksQueue1 = new Queue<dm_scanner_ct>();
        }
        return instance;
      }
    }
    public void InitBindLinsAll(int _id, ref int tonglink) {
      try {
        foreach (dm_scanner_ct item in SQLDatabase.Loaddm_scanner_ct(string.Format("select id, '' as name, path, parentid, statur, dosau  from dm_scanner_ct where [statur]='false' and parentid='{0}'", _id))) {
          LinksQueue1.Enqueue(item);
        }
        DataTable tb = SQLDatabase.ExcDataTable(string.Format("select count(*) as tongcong from dm_scanner_ct where parentid='{0}'", _id));
        tonglink = ConvertType.ToInt(tb.Rows[0]["tongcong"]);


      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message, "BindIntLinsAll");
      }
    }

    public int CountQueue1() {
      return LinksQueue1.Count();
    }


    public int DaXuLy() {
      return daxuly;
    }

    /// <summary>
    /// The single instance method that writes to the log file
    /// </summary>
    /// <param name="message">The message to write to the log</param>
    public bool EnqueueLinks1(dm_scanner_ct dm) {
      try {
        lock (LinksQueue1) {
          DataTable tb = SQLDatabase.ExcDataTable(string.Format("select count(*) as tongcong from dm_scanner_ct where [parentid]='{0}' and path='{1}'", dm.parentid, dm.path));
          if (ConvertType.ToInt(tb.Rows[0]["tongcong"]) == 0) {
            if (SQLDatabase.Adddm_scanner_ct1(dm)) {
              LinksQueue1.Enqueue(dm);
              return true;
            }
            return false;
          }
          return false;
        }
      }
      catch (Exception ex) {
        return false;
      }

    }

    /// <summary>
    /// Flushes the Queue to the physical log file
    /// </summary>
    public dm_scanner_ct DequeueLinks1() {
      dm_scanner_ct entry = new dm_scanner_ct();
      if (LinksQueue1.Count() > 0) {
        if (LinksQueue1.Any()) {
          entry = LinksQueue1.Dequeue();
          SQLDatabase.ExcNonQuery(string.Format("update [dbo].[dm_scanner_ct]  set [statur]=1 where id='{0}' ", entry.id));
        }
      }
      return entry;
    }
  }


  #endregion


  #region Queue

  /// <summary>
  /// A Logging class implementing the Singleton pattern and an internal Queue to be flushed perdiodically
  /// http://zurb.com/forrst/posts/A_simple_C_Thread_Safe_Logging_Class-wA2
  /// </summary>
  public class LogWriter {
    private static LogWriter instance;
    private static Queue<Log> logQueue;
    private static string logDir = "D";//<Path to your Log Dir or Config Setting>;
    private static string logFile = "log.txt";//<Your Log File Name or Config Setting>;
    private static int maxLogAge = int.Parse("5000");
    private static int queueSize = int.Parse("0");
    private static DateTime LastFlushed = DateTime.Now;

    /// <summary>
    /// Private constructor to prevent instance creation
    /// </summary>
    private LogWriter() { }

    /// <summary>
    /// An LogWriter instance that exposes a single instance
    /// </summary>
    public static LogWriter Instance {
      get {
        // If the instance is null then create one and init the Queue
        if (instance == null) {
          instance = new LogWriter();
          logQueue = new Queue<Log>();
        }
        return instance;
      }
    }

    /// <summary>
    /// The single instance method that writes to the log file
    /// </summary>
    /// <param name="message">The message to write to the log</param>
    public void WriteToLog(string message) {
      // Lock the queue while writing to prevent contention for the log file
      lock (logQueue) {
        // Create the entry and push to the Queue
        Log logEntry = new Log(message);
        logQueue.Enqueue(logEntry);

        // If we have reached the Queue Size then flush the Queue
        if (logQueue.Count >= queueSize || DoPeriodicFlush()) {
          FlushLog();
        }
      }
    }

    private bool DoPeriodicFlush() {
      TimeSpan logAge = DateTime.Now - LastFlushed;
      if (logAge.TotalSeconds >= maxLogAge) {
        LastFlushed = DateTime.Now;
        return true;
      }
      else {
        return false;
      }
    }

    /// <summary>
    /// Flushes the Queue to the physical log file
    /// </summary>
    private void FlushLog() {
      while (logQueue.Count > 0) {
        Log entry = logQueue.Dequeue();
        string logPath = logDir + entry.LogDate + "_" + logFile;

        // This could be optimised to prevent opening and closing the file for each write
        using (FileStream fs = File.Open(logPath, FileMode.Append, FileAccess.Write)) {
          using (StreamWriter log = new StreamWriter(fs)) {
            log.WriteLine(string.Format("{0}\t{1}", entry.LogTime, entry.Message));
          }
        }
      }
    }
  }

  /// <summary>
  /// A Log class to store the message and the Date and Time the log entry was created
  /// </summary>
  public class Log {
    public string Message { get; set; }
    public string LogTime { get; set; }
    public string LogDate { get; set; }

    public Log(string message) {
      Message = message;
      LogDate = DateTime.Now.ToString("yyyy-MM-dd");
      LogTime = DateTime.Now.ToString("hh:mm:ss.fff tt");
    }
  }
  #endregion
  public static class Helpers {
    /*https://forum.unity.com/threads/c-how-to-clear-one-line-of-a-text-object-after-it-has-x-lines-is-full.307931/*/
    public static List<string> list = new List<string>();
    public static RichTextBox shownText { get; set; }
    private static string wholeText;
    private static string datum;
    private static int maxLines = 4;
    private static string strDate;
    private static int n;


    /*************************************/

    public static void addText(string newInput) {
      if (list.Count == maxLines) {
        list.RemoveAt(0);
      }
      // datum = Date.datum;
      newInput = (datum + " " + newInput);
      list.Add(newInput);
      UpdateText();
    }

    private static void UpdateText() {
      try {
        wholeText = "";
        foreach (string s in list) {
          strDate = s as string;
          wholeText = (wholeText + " " + strDate);
        }
        shownText.Invoke((Action)delegate {
          shownText.Text = wholeText;
          shownText.Update();
        });
      }
      catch (Exception ex) {

      }

    }

    private static LogWriter writer;
    public static int vitritim(List<string> array, string xx) {
      try {
        int i = 0;
        foreach (var bale in array) {
          if (bale.Contains(xx)) {
            return i;
          }
          i++;
        }
        return -1;
      }
      catch (Exception ex) {
        return -1;
      }
    }
    public static int vitritim(List<string> array, string xx, int vt) {
      try {
        if (vt == array.Count()) return -1;
        vt++;
        for (int i = vt; i < array.Count(); i++) {
          if (array[i].Contains(xx))
            return i;
        }
        return -1;
      }
      catch (Exception ex) {
        return -1;
      }
    }


    //<INPUT type=hidden value=011 name=code>
    public static string getValueHtml(string tabHTML) {
      //b1: cho tất cả vào mảng cách nhau bởi khoản trắng
      string[] arrGetHtmlDocument = tabHTML.Trim().Split(' ');
      //b2: tìm mảng nào có chứa value
      string giatri = Array.Find(arrGetHtmlDocument, n => n.Contains("value"));
      //b3: cắt giatri có trong string ra
      // Location of the letter c.
      int i_dau = giatri.IndexOf("'") + 1;
      int i_cuoi = giatri.LastIndexOf("'");
      string d = giatri.Substring(i_dau, i_cuoi - i_dau);
      return d;
    }


    /// <param name="chuoi1"></param>
    /// <param name="chuoi2"></param>
    /// <param name="and_or">and_or: and =true; or= false</param>
    /// <returns></returns>
    public static int vitritim(List<string> array, string chuoi1, string chuoi2, bool and_or) {
      try {
        int i = 0;
        foreach (var bale in array) {

          if (and_or && (bale.Contains(chuoi1) && bale.Contains(chuoi2))) {
            return i;
          }
          if (!and_or && (bale.Contains(chuoi1) || bale.Contains(chuoi2))) {
            return i;
          }
          i++;
        }
        return -1;
      }
      catch (Exception ex) {
        return -1;
      }
    }



    public static List<string> getUrlHtml2(string html) {
      try {
        List<string> xx = new List<string>();
        while (html.IndexOf("href=\"") != -1) {
          int vt1 = html.IndexOf("href=\"") + 6;
          int vt2 = 0;
          for (int i = vt1; i < html.Length; i++) {
            if (html[i].Equals('"')) {
              vt2 = i;
              break;
            }
          }

          xx.Add(html.Substring(vt1, (vt2 - vt1)));
          html = html.Substring(vt2, html.Length - vt2);
        }
        return xx;
      }
      catch (Exception) {

        return null;
      }

    }

    public static List<string> getUrlHtml3(string html) {
      try {
        List<string> xx = new List<string>();
        while (html.IndexOf("href=\'") != -1) {
          int vt1 = html.IndexOf("href=\'") + 6;
          int vt2 = 0;
          for (int i = vt1; i < html.Length; i++) {
            if (html[i].Equals('\'')) {
              vt2 = i;
              break;
            }
          }

          xx.Add(html.Substring(vt1, (vt2 - vt1)));
          html = html.Substring(vt2, html.Length - vt2);
        }
        return xx;
      }
      catch (Exception) {

        return null;
      }

    }

    public static string getUrlHtml(string tabHTML) {
      //b1: cho tất cả vào mảng cách nhau bởi khoản trắng
      string[] arrGetHtmlDocument = tabHTML.Trim().Split(' ');
      //b2: tìm mảng nào có chứa value
      string giatri = Array.Find(arrGetHtmlDocument, n => n.Contains("href"));
      //b3: cắt giatri có trong string ra
      // Location of the letter c.
      int i_dau = giatri.IndexOf("\"") + 1;
      int i_cuoi = giatri.LastIndexOf("\"");
      string d = giatri.Substring(i_dau, i_cuoi - i_dau);
      return d;
    }
    public static string getClassHtml(string tabHTML) {
      //b1: cho tất cả vào mảng cách nhau bởi khoản trắng
      string[] arrGetHtmlDocument = tabHTML.Trim().Split(' ');
      //b2: tìm mảng nào có chứa value
      string giatri = Array.Find(arrGetHtmlDocument, n => n.Contains("class"));
      //b3: cắt giatri có trong string ra
      // Location of the letter c.
      int i_dau = giatri.IndexOf("\"") + 1;
      int i_cuoi = giatri.LastIndexOf("\"");
      string d = giatri.Substring(i_dau, i_cuoi - i_dau);
      return d;
    }
    public static List<string> getDataHTML(string tabHTML) {
      List<string> arr = new List<string>();
      int vt1 = 0, vt2 = 0;
      try {

        for (int i = 0; i < tabHTML.Length - 1; i++) {
          if (tabHTML[i].Equals('>')) {
            vt1 = i;
            for (int j = i; j < tabHTML.Length; j++) {
              if (tabHTML[j].Equals('<')) {
                vt2 = j;
                break;
              }
            }
            if (vt2 == 0 && tabHTML.Length > 0)/*by luulong:16/06/2017*/
              vt2 = tabHTML.Length;

            if (!string.IsNullOrEmpty(tabHTML.Substring(vt1 + 1, vt2 - vt1 - 1)))
              arr.Add(tabHTML.Substring(vt1 + 1, vt2 - vt1 - 1).Trim());
          }
        }
        return arr;
      }
      catch {
        //writer = LogWriter.Instance;
        //writer.WriteToLog(string.Format("{0}", tabHTML));
        //MessageBox.Show("Lỗi cấu trúc không đúng Vui lòng nhấn enter để tiếp tục\n " + ex.Message, "getHTML");
        return arr;
      }
    }
    public static void LogMessage(string msg) {
      string sFilePath = Environment.CurrentDirectory + "Log_" + System.AppDomain.CurrentDomain.FriendlyName + ".txt";

      System.IO.StreamWriter sw = System.IO.File.AppendText(sFilePath);
      try {
        string logLine = System.String.Format(
            "{0:G}: {1}.", System.DateTime.Now, msg);
        sw.WriteLine(logLine);
      }
      finally {
        sw.Close();
      }
    }
    public static List<string> FindAllPhone(string tabHTML) {
      try {
        List<string> danhsachdienthoai = new List<string>();
        string[] danhsachdauso = { "09", "01" };
        foreach (var item_dauso in danhsachdauso) {
          int vt1 = 0;
          while (vt1 <= tabHTML.Length) {
            vt1 = tabHTML.IndexOf(item_dauso, vt1);
            if (vt1 == -1)
              break;
            string chuoitamlay = tabHTML.Substring(vt1, 20);
            string dienthoai = "";
            foreach (var item in chuoitamlay) {
              if (System.Text.RegularExpressions.Regex.IsMatch(item.ToString(), "[0-9]"))
                dienthoai += item;
            }

            if (item_dauso == "09") {
              if (dienthoai.Length >= 10) {
                danhsachdienthoai.Add(dienthoai.Substring(0, 10));
                vt1 = vt1 + 10;
              }
              else {
                vt1 = vt1 + dienthoai.Length;
              }
            }
            if (item_dauso == "01") {
              if (dienthoai.Length >= 11) {
                danhsachdienthoai.Add(dienthoai.Substring(0, 11));
                vt1 = vt1 + 11;
              }
              else {
                vt1 = vt1 + dienthoai.Length;
              }
            }
          }
        }
        return danhsachdienthoai;
      }
      catch (Exception ex) {
        return null;
      }
    }
    public static string FindAllEmail(string tabHTML) {
      int vt1 = 0;
      try {
        tabHTML = tabHTML.Replace(":&nbsp;", "").Replace("&nbsp;", "").Trim();
        List<string> danhsachemail = new List<string>();
        vt1 = tabHTML.IndexOf("@", vt1);
        int i_dau = tabHTML.LastIndexOf(" ", vt1);
        int i_cuoi = tabHTML.IndexOf(" ", vt1);
        if (i_dau == -1)
          i_dau = 0;
        if (i_cuoi == -1)
          i_cuoi = tabHTML.Length;
        string email = tabHTML.Substring(i_dau, i_cuoi - i_dau);
        return email;

      }
      catch (Exception ex) {
        return "";
      }
    }

    public static bool islink(string link) {
      try {
        if (link.Length < 8)
          return false;
        if (link.StartsWith("http://") || link.StartsWith("https://")) {
          return true;
        }
        else {
          return false;
        }

      }
      catch (Exception ex) {
        return false;
      }
    }

    public static bool IsNumber(string s) {
      return s.All(char.IsDigit);
    }
    public static string getDomain(string link) {
      try {
        int i = 0;
        for (i = link.IndexOf("//") + 2; i < link.Length; i++) {
          if (link[i] == '/')
            break;
        }

        return link.Substring(0, i);
      }
      catch {
        return "";
      }
    }

    public static string getTitleWeb(string html) {
      try {
        int vt1 = html.IndexOf("<title>");
        if (vt1 == -1)
          return "";
        vt1 = vt1 + 7;
        int vt2 = html.IndexOf("</title>", vt1);
        return html.Substring(vt1, vt2 - vt1);
      }
      catch {
        return "";
      }
    }

    public static string convertToUnSign3(string s) {
      Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
      string temp = s.Normalize(NormalizationForm.FormD);
      return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
    }
  }

  public static class Utilities_hosocongty {
    public static bool hasProcess = true; /*khai bao bien stop*/

    public static int _PathLimit;/*so trang quet gioi han*/
    public static int _lanquetlai;
    public static int _Sleep;
    public static int _Timeout;
    public static bool ChkCKiemTraTrung = false;
    public static int IdDanhmuc;
    public static int _thanhcong = 0;
    public static int _thatbai = 0;

    private static LogWriter writer;

    //<INPUT type=hidden value=011 name=code>
    //<INPUT type=hidden value=3 name=id>
    //<INPUT type=hidden value=0 name=last_id>
    //<INPUT type=hidden value=1 name=page>
    //<INPUT type=hidden value=15 name=album_num> 
    /// <summary>
    ///    Lấy thông tin địa chỉ trang kế tiếp    
    /// </summary>
    /// <param name="arrGetHtmlDocument">Thông tin cả trang html</param>
    /// <returns></returns>
    public static PathNext getPageNow(string[] arrGetHtmlDocument) {
      PathNext next = new PathNext();
      //<DIV class=finish><SPAN>----End----</SPAN></DIV></DIV></DIV><INPUT type=hidden value=011 name=code><INPUT type=hidden value=3 name=id><INPUT type=hidden value=0 name=last_id><INPUT type=hidden value=1 name=page><INPUT type=hidden value=15 name=album_num> </DIV></DIV>
      string match = Array.Find(arrGetHtmlDocument, n => n.Contains("hidden") && n.Contains("last_id"));     // Jack
                                                                                                             //Lấy thông tin trang cuối
      string[] infopath = match.Trim().Split('<');
      /*get code;công them < do doan tren mất <*/
      string code = "<" + Array.Find(infopath, n => n.Contains("hidden") && n.Contains("name='code'"));
      next.code = Helpers.getValueHtml(code);

      string id = "<" + Array.Find(infopath, n => n.Contains("hidden") && n.Contains("name='id'"));
      next.id = Helpers.getValueHtml(id);

      string last_id = "<" + Array.Find(infopath, n => n.Contains("hidden") && n.Contains("name='last_id'"));
      next.last_id = Helpers.getValueHtml(last_id);

      string page = "<" + Array.Find(infopath, n => n.Contains("hidden") && n.Contains("name='page'"));
      next.page = Helpers.getValueHtml(page);

      string album_num = "<" + Array.Find(infopath, n => n.Contains("hidden") && n.Contains("name='album_num'"));
      next.album_num = Helpers.getValueHtml(album_num);

      return next;
    }
    ////http://www.hosocongty.vn/json/nganh.php?page=3&last_id=1372716&code=011
    public static PathNext getjsonPathNext(PathNext next_old, Label lbl, Label lblketnoi) {
      HttpWebRequest req;
      HttpWebResponse resp;
      StreamReader reader;
      StringBuilder output;
      string link = "http://www.hosocongty.vn";

      try {
        #region danh sách linh loi
        /**************************************************************************************
         * danh sách các link lổi
         * -1/Lổi last_id null
         *  http://www.hosocongty.vn/json/nganh.php?page=483&last_id=872892&code=0118
         *   http://www.hosocongty.vn/nganh.php?code=0118&id=11&curpage=483&last_id=872892
         **************************************************************************************/
        #endregion
        string strPath = string.Format("http://www.hosocongty.vn/json/nganh.php?page={0}&last_id={1}&code={2}", next_old.page, next_old.last_id, next_old.code);

        req = (HttpWebRequest)WebRequest.Create(strPath);
        resp = (HttpWebResponse)req.GetResponse();

        reader = new StreamReader(resp.GetResponseStream());
        output = new StringBuilder();
        output.Append(reader.ReadToEnd());

        JObject jsonData = JObject.Parse(output.ToString().Replace("\"id\":,", "\"id\":\"-1\",")
                                                          .Replace("\"last_id\":}", "\"last_id\":-1}"));
        json_data myProduct = JsonConvert.DeserializeObject<json_data>(jsonData.ToString());

        for (int i = 0; i < Convert.ToInt32(myProduct.total); i++) {
          if (myProduct.album[i].id != "-1") {
            if (hasProcess) {
              hosocongty1 hs = new hosocongty1();
              hs.danhmucid = IdDanhmuc;
              hs.web_nguon_code = next_old.code;
              hs.ms_thue = myProduct.album[i].mst.Trim();
              hs.web_nguon_id = myProduct.album[i].id.ToString();
              hs.web_nguon_url = myProduct.album[i].url;

              int solanlaplai = 0;
              getChiTiet(string.Format(link + "{0}", hs.web_nguon_url.Substring(1, hs.web_nguon_url.Length - 1)), ref hs, ref solanlaplai, lblketnoi);

              if (!ChkCKiemTraTrung) {
                if (SQLDatabase.AddHosocongty(hs))   /*lưu thông tin vào csdl*/
                  _thanhcong++;
                else
                  _thatbai++;
              }
              else/*có yêu cầu kiễm tra trùng*/
              {
                if (ConvertType.ToInt(SQLDatabase.ExcScalar(string.Format("select count(*) from hosocongty where danhmucId='{0}' and ms_thue='{1}'", hs.danhmucid, hs.ms_thue))) == 0) {
                  if (SQLDatabase.AddHosocongty(hs))   /*lưu thông tin vào csdl*/
                    _thanhcong++;
                  else
                    _thatbai++;
                }

              }
              ShowMessage(lbl, next_old.page, hs.web_nguon_url, _thanhcong, _thatbai);
            }
          }
        }
        if (Convert.ToInt32(myProduct.total) != 0) {
          /*lấy thông tin trang kế*/
          PathNext next_new = (PathNext)next_old.Clone();
          next_new.last_id = myProduct.last_id;
          next_new.page = (Convert.ToInt32(next_new.page) + 1).ToString();
          return next_new;
        }

        return next_old;
      }
      catch (Exception ex) {
        writer = LogWriter.Instance;
        writer.WriteToLog(string.Format("{0}-{1}-{2}", ex.Message, "getjsonPathNext", "error1"));
        _thatbai++;
        return next_old;
      }
    }
    private static void getChiTiet(string url, ref hosocongty1 hs, ref int solanlap, Label lblketnoi) {
      HttpWebRequest req;
      HttpWebResponse resp;
      StreamReader reader;
      StringBuilder output;
      string[] arrGetDocument;
      int vt;

      try {


        req = (HttpWebRequest)WebRequest.Create(url);
        req.Timeout = _Timeout;
        req.ReadWriteTimeout = _Timeout;

        resp = (HttpWebResponse)req.GetResponse();

        reader = new StreamReader(resp.GetResponseStream());
        output = new StringBuilder();
        output.Append(reader.ReadToEnd());


        string title = Helpers.getTitleWeb(output.ToString());
        if (title == "") {  /*truong hop vat gia loi connect*/
          if (solanlap <= _lanquetlai) {
            solanlap = solanlap + 1;
            lblketnoi.Visible = true;
            lblketnoi.Text = string.Format("Đứt Kết Nối : + {0}", solanlap - 1);
            lblketnoi.Update();
            Thread.Sleep(_Sleep);
            getChiTiet(url, ref hs, ref solanlap, lblketnoi);
          }
          else
            _thatbai++;
        }

        lblketnoi.Visible = false;
        lblketnoi.Update();

        arrGetDocument = output.ToString().Split(new char[] { '\n', '\r' });

        var index = Array.Find(arrGetDocument, x => x.Contains("Thông tin chi tiết"));

        List<string> xxx = Helpers.getDataHTML(index.ToString());     //http://prntscr.com/cczwog
                                                                      /*tim ten quoc te*/
        vt = Helpers.vitritim(xxx, "Tên quốc tế:");
        if (vt != -1)
          hs.ten_quoc_te = xxx[vt + 1];
        /*ten giao dich*/
        vt = Helpers.vitritim(xxx, "Tên giao dịch:");
        if (vt != -1)
          hs.ten_giao_dich = xxx[vt + 1];
        /*ma so thue + ten giao dich */
        vt = Helpers.vitritim(xxx, "Mã số thuế:");
        if (vt != -1) {
          hs.ten_cong_ty = xxx[vt - 1];
        }
        vt = Helpers.vitritim(xxx, "Địa chỉ:");
        if (vt != -1) {
          hs.dia_chi = xxx[vt + 1];
        }
        vt = Helpers.vitritim(xxx, "ngày cấp:");
        if (vt != -1) {
          if (xxx[vt].Length > 10) {
            hs.ngay_cap = xxx[vt].Substring(xxx[vt].Length - 10, 10);
          }
        }
        vt = Helpers.vitritim(xxx, "Ngày hoạt động:");
        if (vt != -1) {
          if (xxx[vt].Length > 10) {
            hs.ngay_hoat_dong = xxx[vt].Substring(xxx[vt].Length - 10, 10);
          }
        }
        vt = Helpers.vitritim(xxx, "Điện thoại:");
        if (vt != -1) {
          hs.dien_thoai = xxx[vt + 1];
        }
        vt = Helpers.vitritim(xxx, "Fax:");
        if (vt != -1) {
          hs.fax = xxx[vt + 1];
        }
        vt = Helpers.vitritim(xxx, "Giám đốc:");
        if (vt != -1) {
          hs.giam_doc = xxx[vt + 1];
        }
        vt = Helpers.vitritim(xxx, "Giấy phép kinh doanh:");
        if (vt != -1) {
          hs.gp_kinh_doanh = xxx[vt + 1];
        }
        vt = Helpers.vitritim(xxx, "Website:");/*trường mã hoá*/
        if (vt != -1) {
          if (xxx[vt + 2] == "[email&#160;protected]" && xxx[vt + 1] == "http:")
            hs.website_cty = xxx[vt + 1] + getGiaiMa(output.ToString(), "Website:");
          else
            hs.website_cty = xxx[vt + 1];
        }
        vt = Helpers.vitritim(xxx, "Email:");/*trường mã hoá*/
        if (vt != -1) {

          if (xxx[vt + 1] == "[email&#160;protected]")
            hs.email_cty = getGiaiMa(output.ToString(), "Email:");
          else
            hs.email_cty = xxx[vt + 1];
        }
        vt = Helpers.vitritim(xxx, "Ngân hàng:");
        if (vt != -1) {
          hs.ngan_hang = xxx[vt + 1];
        }
        vt = Helpers.vitritim(xxx, "Số TK:");
        if (vt != -1) {
          hs.so_tai_khoan = xxx[vt + 1];
        }
        vt = Helpers.vitritim(xxx, "Đã đóng mã số thuế ngày");
        if (vt != -1) {
          hs.ngay_dong_cua = xxx[vt + 1];
        }

        /*lấy ngành nghề chính*/
        var index1 = Array.Find(arrGetDocument, x => x.Contains("boxNg"));
        string chuoimoi2 = "<a href='nganh.php?code=";
        List<string> arrGetDocument1 = index1.Split(new string[] { chuoimoi2 }, StringSplitOptions.None).Where(x => x.Contains("<td align='center'>")).ToList();
        if (arrGetDocument1.Count() > 0) {
          string chuoinganhnghe = arrGetDocument1[0];
          int vt1 = chuoinganhnghe.IndexOf("&");
          hs.nganh_nghe_chinh = chuoinganhnghe.Substring(0, vt1);
        }


      }
      catch (WebException ex) {
        if (ex.ToString().Contains("time")) {
          if (solanlap <= _lanquetlai && _lanquetlai != -1) {
            solanlap = solanlap + 1;
            lblketnoi.Visible = true;
            lblketnoi.Text = string.Format("Đứt Kết Nối : + {0}", solanlap - 1);
            lblketnoi.Update();
            Thread.Sleep(_Sleep);
            getChiTiet(url, ref hs, ref solanlap, lblketnoi);
          }
          if (_lanquetlai == -1) {
            lblketnoi.Visible = true;
            lblketnoi.Text = string.Format("Đứt Kết Nối");
            lblketnoi.Update();
            Thread.Sleep(_Sleep);
            solanlap = 0;
            getChiTiet(url, ref hs, ref solanlap, lblketnoi);
          }
          else
            _thatbai++;
        }
      }
      catch (Exception ex) {
        return;
      }
    }
    public static string getGiaiMa(string html, string tukhoa) {
      try {

        int vt_cat_html_email = html.IndexOf(tukhoa);
        string html2 = html.Substring(vt_cat_html_email, html.Length - vt_cat_html_email);

        /*kiểm tra xem có dùng mã hóa không? tức có tồn tại cfemail không?*/
        //if (html2.IndexOf("data-cfemail=") == -1)
        //    return "";
        int vtdau = html2.IndexOf("data-cfemail=") + 14;
        string a = html2.Substring(vtdau, 100);
        int vtdau1 = a.IndexOf("\">");
        string aa = a.Substring(0, vtdau1);
        var r = Convert.ToInt32(aa.Substring(0, 2), 16);
        string s = "";
        for (int j = 2; j < vtdau1; j += 2) {
          int c = Convert.ToInt32(aa.Substring(j, 2), 16) ^ r;
          s += Convert.ToChar(c);
        }
        return s;
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message, "getGiaiMa");
        return "";
      }

    }
    public static void getwebBrowser(string strPath, PathNext next_old, ref int solanlap, Label lbl, Label lblketnoi) {
      HttpWebRequest req;
      HttpWebResponse resp;
      StreamReader reader;
      StringBuilder output;
      string[] arrGetHtmlDocumentALL;

      try {
        if (!Utilities_hosocongty.hasProcess)
          return;/*stop*/
        req = (HttpWebRequest)WebRequest.Create(strPath);
        req.Timeout = _Timeout;
        req.ReadWriteTimeout = _Timeout;

        resp = (HttpWebResponse)req.GetResponse();

        reader = new StreamReader(resp.GetResponseStream());
        output = new StringBuilder();

        output.Append(reader.ReadToEnd());

        lblketnoi.Visible = true;
        lblketnoi.Update();

        arrGetHtmlDocumentALL = output.ToString().Split('\n');

        if (next_old == null)
          next_old = Utilities_hosocongty.getPageNow(arrGetHtmlDocumentALL);

        if (ConvertType.ToInt(next_old.page) > _PathLimit && _PathLimit != -1)
          return;

        PathNext next_new = Utilities_hosocongty.getjsonPathNext(next_old, lbl, lblketnoi);

        if (next_old != next_new && next_new.last_id != "-1")/*-1 lổi cấu trúc json=> dẩn den khong biết mã ke tiep*/
        {
          string diachi_new = string.Format("http://www.hosocongty.vn/nganh.php?code={0}&id={1}&curpage={2}&last_id={3}", next_new.code, next_new.id, next_new.page, next_new.last_id);
          int solanlap1 = 0;
          getwebBrowser(diachi_new, next_new, ref solanlap1, lbl, lblketnoi);
        }

      }
      catch (WebException ex) {
        if (ex.ToString().Contains("time")) {
          if (solanlap <= _lanquetlai && _lanquetlai != -1) {
            solanlap = solanlap + 1;
            lblketnoi.Visible = true;
            lblketnoi.Text = string.Format("Đứt Kết Nối : + {0}", solanlap - 1);
            lblketnoi.Update();
            Thread.Sleep(_Sleep);
            getwebBrowser(strPath, next_old, ref solanlap, lbl, lblketnoi);
          }
          else if (_lanquetlai == -1) {
            lblketnoi.Text = string.Format("Đứt Kết Nối");
            lblketnoi.Update();
            Thread.Sleep(_Sleep);
            solanlap = 0;
            getwebBrowser(strPath, next_old, ref solanlap, lbl, lblketnoi);
          }
          else
            _thatbai++;
        }
      }
      catch (Exception e) {
        return;
      }

    }
    //http://www.hosocongty.vn/nganh.php?code=01170&id=10&curpage=2&last_id=1420786
    public static void ShowMessage(Label lbl_thongbao_hosocongty, string trang, string chitiet, int thanhcong, int thatbai) {
      lbl_thongbao_hosocongty.Text = string.Format("Trang: {0} - Thành công: {1}/Thất bại:{2} \n {3}", trang, thanhcong, thatbai, chitiet);
      lbl_thongbao_hosocongty.Update();
    }
    public static DataTable Formatdm_hsct(DataTable tb_goc) {
      try {
        if (tb_goc.Rows.Count == 0)
          return tb_goc;
        DataTable tb_new = tb_goc.Clone();
        tb_new.Clear();
        DataRow[] _tb_cap1 = tb_goc.Select("capid=1");
        foreach (DataRow item in _tb_cap1) {
          tb_new.ImportRow(item);
          DataRow[] _tb_cap2 = tb_goc.Select(string.Format("parentId='{0}' and capid=2", item["Id"].ToString()));
          foreach (DataRow item2 in _tb_cap2) {
            tb_new.ImportRow(item2);
            string xx = string.Format("[spFindDanhMuc] '{0}'", item2["id"].ToString());
            DataTable tb_con = SQLDatabase.ExcDataTable(xx);
            foreach (DataRow item3 in tb_con.Rows)
              tb_new.ImportRow(item3);
          }
          /*cac cap con lai*/
          DataRow[] _tb_cap3_4_5 = tb_goc.Select(string.Format("parentId='{0}' and capid in(3,4,5)", item["Id"].ToString()));
          foreach (DataRow item2 in _tb_cap3_4_5) {
            tb_new.ImportRow(item2);
            string xx = string.Format("[spFindDanhMuc] '{0}'", item2["id"].ToString());
            DataTable tb_con = SQLDatabase.ExcDataTable(xx);
            foreach (DataRow item3 in tb_con.Rows)
              tb_new.ImportRow(item3);
          }
        }
        return tb_new;
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message, "FormatDanhMuc");
        return null;
      }
    }

  }
  public static class Utilities_vatgia {
    public static bool hasProcess = true; /*khai bao bien stop*/

    public static int _PathLimit;/*so trang quet gioi han*/
    public static int _lanquetlai;
    public static int _Sleep;
    public static int _Timeout;


    public static int IdDanhmuc;
    public static int _thanhcong = 0;
    public static int _thatbai = 0;
    public static Dictionary<string, int> _dau_so;
    public static List<regexs> _regexs;

    private static LogWriter writer;

    public static string WebRequestNavigate(string url, Label lbl) {
      Stream stream = null;
      StringBuilder output = new StringBuilder();

      while (stream == null && hasProcess) {
        try {

          StreamReader reader;
          HttpWebResponse resp = null;
          HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);


          resp = (HttpWebResponse)myReq.GetResponse();
          stream = resp.GetResponseStream();

          reader = new StreamReader(resp.GetResponseStream());
          output.Clear();
          output.Append(reader.ReadToEnd());

          if (output.Length <= 400) {

            if (lbl != null) {
              lbl.Visible = true;
              lbl.Update();
              if (lbl.Text.Length < 20) {
                lbl.Text = lbl.Text + "*";

              }
              else {
                lbl.Text = "Khoá IP";
              }
              lbl.Update();
            }
            System.Threading.Thread.Sleep(5000);
            stream = null;
          }
        }
        catch (Exception e) {
          System.Threading.Thread.Sleep(5000);
        }

      }
      if (lbl != null) {
        lbl.Visible = false;
        lbl.Update();
      }

      return output.ToString();
    }

    public static void getwebBrowser(string strPath, ref int solanlap, Label lbl, Label lblkhoa) {
      HttpWebRequest req;
      HttpWebResponse resp;
      StreamReader reader;
      StringBuilder output;
      string[] arrGetHtmlDocumentALL;
      try {

        if (!Utilities_vatgia.hasProcess)
          return;/*stop*/

        req = (HttpWebRequest)WebRequest.Create(strPath);
        if (_Timeout > 0)
          req.Timeout = _Timeout;

        resp = (HttpWebResponse)req.GetResponse();

        reader = new StreamReader(resp.GetResponseStream());
        output = new StringBuilder();
        output.Append(reader.ReadToEnd());

        string title = Helpers.getTitleWeb(output.ToString());
        if (title == "") {  /*truong hop vat gia loi connect*/
          if (solanlap <= _lanquetlai) {
            solanlap = solanlap + 1;
            lblkhoa.Visible = true;
            lblkhoa.Update();
            Thread.Sleep(_Sleep);
            getwebBrowser(strPath, ref solanlap, lbl, lblkhoa);
          }
          else
            _thatbai++;
        }

        string strHtml = output.ToString();
        arrGetHtmlDocumentALL = strHtml.Split('\n');

        string chuoimoi1 = "<table class=\"shop_table\"";
        int vt1 = strHtml.ToString().IndexOf(chuoimoi1);
        string str1 = strHtml.ToString().Substring(vt1, strHtml.ToString().Length - vt1);
        int vt2 = str1.IndexOf("</table>");
        string str2 = str1.Substring(0, vt2 + chuoimoi1.Length);

        string chuoimoi2 = "<tr class=\"tr\">";
        string[] arrGetDocument = str2.Split(new string[] { chuoimoi2 }, StringSplitOptions.None);

        int i = 0;
        vatgia vg;
        foreach (string item in arrGetDocument) {
          if (Utilities_vatgia.hasProcess) {
            string chuoimoi3 = "<div";
            string[] xxx = item.Split(new string[] { chuoimoi3 }, StringSplitOptions.None);
            int vtri = -1;
            string urlchitiet = "";

            if (i != 0) {
              vg = new vatgia();
              vg.danhmucid = IdDanhmuc;
              List<string> kq = xxx.Select(x => string.Format("<div {0}", x)).ToList();
              vtri = Helpers.vitritim(kq, "company_name");
              if (vtri != -1) {
                vg.tencongty = Helpers.getDataHTML(kq[vtri].Replace("\r", "").ToString()).FirstOrDefault();
                urlchitiet = Helpers.getUrlHtml(kq[vtri]).Replace(" ", "").Replace("\t", "");
                string urlcon = string.Format("http://www.vatgia.com/{0}&module=contact", urlchitiet);
                vg.sys_diachiweb = urlcon;
                ShowMessage(lbl, urlcon, _thanhcong, _thatbai);

                int solanlap1 = 0;
                getTrangCon(urlcon, ref vg, ref solanlap1, lblkhoa);
              }

              //address
              vtri = Helpers.vitritim(kq, "address");
              if (vtri != -1) {
                vg.dia_chi = Helpers.getDataHTML(kq[vtri].Replace("\r", "").ToString()).FirstOrDefault();
              }
              //website
              vtri = Helpers.vitritim(kq, "website");
              if (vtri != -1) {
                vg.website = Helpers.getDataHTML(kq[vtri].Replace("\r", "").ToString()).FirstOrDefault();
              }
              //website
              vtri = Helpers.vitritim(kq, "yahoo");
              if (vtri != -1) {
                vg.yahoo = Helpers.getDataHTML(kq[vtri].Replace("\r", "").ToString()).FirstOrDefault();
              }
              //phone
              vtri = Helpers.vitritim(kq, "phone");
              if (vtri != -1) {
                string dienthoai = Helpers.getDataHTML(kq[vtri].Replace("\r", "").ToString()).FirstOrDefault();
                dienthoai = dienthoai.Replace("Điện thoại :", "");
                //string[] delimiterChars = { "/", " - "};
                string[] dienthoais = dienthoai.Split(new string[] { "/", " - " }, StringSplitOptions.None);
                if (dienthoais.Length == 1) {
                  vg.hotline1 = dienthoais[0].Trim();
                }
                else if (dienthoais.Length == 2) {
                  vg.hotline1 = dienthoais[0].Trim();
                  vg.hotline2 = dienthoais[1].Trim();
                }
              }
              //<div  class="fax">Fax : 08-62971471</div>
              vtri = Helpers.vitritim(kq, "fax");
              if (vtri != -1) {
                string fax = Helpers.getDataHTML(kq[vtri].Replace("\r", "").ToString()).FirstOrDefault();
                vg.fax = fax.Replace("Fax :", "").Trim();
              }

              if (Utilities_vatgia.hasProcess)/*nếu đang chạy mới được phép lưu thông tin, còn đã dừng tiến trình thi không được phép lưu*/
                if (SQLDatabase.AddVatGia(vg))   /*lưu thông tin vào csdl*/
                  _thanhcong++;

            }
          }
          i++;
        }

      }
      catch {
        if (solanlap <= _lanquetlai) {
          solanlap = solanlap + 1;
          lblkhoa.Visible = true;
          lblkhoa.Update();
          Thread.Sleep(_Sleep);
          getwebBrowser(strPath, ref solanlap, lbl, lblkhoa);
        }
        else
          _thatbai++;
      }
    }

    public static void ShowMessage(Label lbl, string chitiet, int thanhcong, int thatbai) {
      lbl.Text = string.Format("Thành công: {0} - Thất Bại: {1}  / {2}", thanhcong, thatbai, chitiet);
      lbl.Update();
    }

    public static void getTrangCon(string strPath, ref vatgia vg, ref int solanlap, Label lbl) {
      HttpWebRequest req;
      HttpWebResponse resp;
      StreamReader reader;
      StringBuilder output;
      string strWebsite;

      try {

        if (!Utilities_vatgia.hasProcess)
          return;/*stop*/

        req = (HttpWebRequest)WebRequest.Create(strPath);
        if (_Timeout > 0)
          req.Timeout = _Timeout;

        resp = (HttpWebResponse)req.GetResponse();

        reader = new StreamReader(resp.GetResponseStream());
        output = new StringBuilder();
        output.Append(reader.ReadToEnd());


        string title = Helpers.getTitleWeb(output.ToString());
        if (title == "") {  /*truong hop vat gia loi connect*/
          if (solanlap <= _lanquetlai) {
            solanlap = solanlap + 1;
            lbl.Visible = true;
            lbl.Update();
            Thread.Sleep(_Sleep);
            getTrangCon(strPath, ref vg, ref solanlap, lbl);
          }
          else
            _thatbai++;
        }

        strWebsite = output.ToString();

        string strdiemmoc1 = "<div class=\"padding module_larger_border\">";
        string strdiemmoc2 = "<div class=\"module_bottom\">";
        int vt1 = strWebsite.IndexOf(strdiemmoc1);
        // if (vt1 == -1) return; /*truong hop website bi đổi hướng*/
        int vt2 = strWebsite.IndexOf(strdiemmoc2, vt1);

        string strLienheHTML = strWebsite.Substring(vt1, vt2 - vt1);

        List<string> strLienHeArr = Helpers.getDataHTML(strLienheHTML);
        if (strLienHeArr.Count == 0)
          return;
        string strLienHe = "";
        foreach (var item in strLienHeArr) {
          strLienHe = strLienHe + item + "\n";
        }
        vg.lienhe = strLienHe;

        /*******************************************************/

        List<string> dsdienthoai = Utilities_scanner.getPhoneHTML(strLienHeArr, _dau_so, _regexs);

        vg.didong1 = dsdienthoai.Count() >= 1 ? dsdienthoai[0].ToString() : "";
        vg.didong2 = dsdienthoai.Count() >= 2 ? dsdienthoai[1].ToString() : "";
        vg.didong3 = dsdienthoai.Count() >= 3 ? dsdienthoai[2].ToString() : "";

        List<string> dsemail = Utilities_scanner.getEmail(strLienHeArr);

        vg.email1 = dsemail.Count() >= 1 ? dsemail[0].ToString() : "";
        vg.email2 = dsemail.Count() >= 2 ? dsemail[1].ToString() : "";
        vg.email3 = dsemail.Count() >= 3 ? dsemail[2].ToString() : "";
      }
      catch {
        if (solanlap <= _lanquetlai) {
          solanlap = solanlap + 1;
          lbl.Visible = true;
          lbl.Update();
          Thread.Sleep(_Sleep);
          getTrangCon(strPath, ref vg, ref solanlap, lbl);
        }
        else
          _thatbai++;
      }
    }
    public static int getPageMax(string url, Label lblkhoa) {
      string str = "";
      try {
        //url = "http://www.vatgia.com/home/shop.php?view=list&iCat=15448";
        str = Utilities_vatgia.WebRequestNavigate(url, lblkhoa);
        string strdkmoc = "<div class=\"page_div\">";
        int vt1 = str.IndexOf(strdkmoc);
        if (vt1 == -1)
          return 0;
        string chuoixuly1 = str.Substring(vt1, str.Length - vt1);

        string strdkmoc2 = "</div>";
        int vt2 = chuoixuly1.IndexOf(strdkmoc2);
        string chuoixuly2 = chuoixuly1.Substring(0, vt2) + strdkmoc2;

        string[] mang = chuoixuly2.Split(new string[] { "</a>" }, StringSplitOptions.None);

        string giatri = Array.Find(mang, n => n.Contains("\"Trang cuối\"")) == null ?
                       Array.Find(mang, n => n.Contains("\"Trang sau\"")) :
                       Array.Find(mang, n => n.Contains("\"Trang cuối\""));



        int vtdau1 = giatri.IndexOf("page=");
        string xx = "class=\"page\">";
        int vtdau2 = giatri.IndexOf(xx);

        string xxx = giatri.Substring(vtdau1 + 5, (vtdau2 - (vtdau1 + 5)));
        string[] so = xxx.Split(new string[] { "\"" }, StringSplitOptions.None);

        if (so.Length == 1)
          return -1;
        return ConvertType.ToInt(so[0]);
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message, "getPageNext");
        return -1;
      }
    }
  }
  public static class Utilities_scanner {
    public static bool hasProcess = true; /*khai bao bien stop*/
    public static int _dosau;/*so trang quet gioi han*/
    public static int _sleep;/*so trang quet gioi han*/
    public static int _timeout;
    public static int _gioihan_lienket;
    public static string _doman;
    public static string _nhom;
    public static dm_scanner_ct _dmchinh;
    public static List<regexs> _regexs;
    public static int _lanquetlai;
    public static Dictionary<string, int> _dau_so;
    public static int _slPhone;
    public static int _slEmail;

    public static bool _emails;
    public static bool _sodienthoai;
    public static string _title;
    public static int _tonglink;
    public static bool _HienThiChiTietQuet;
    public static string _UserAgent;


    private static LogWriter writer;



    /// <summary>
    /// Remove HTML from string with Regex.
    /// </summary>
    public static string StripTagsRegex(string source) {
      return Regex.Replace(source, "<.*?>", string.Empty);
    }


    /// <summary>
    /// Remove HTML tags from string using char array.
    /// </summary>
    public static string StripTagsCharArray(string source) {
      try {
        char[] array = new char[source.Length];
        int arrayIndex = 0;
        bool inside = false;

        for (int i = 0; i < source.Length; i++) {
          char let = source[i];
          if (let == '<') {
            inside = true;
            continue;
          }
          if (let == '>') {
            inside = false;
            continue;
          }
          if (!inside) {
            array[arrayIndex] = let;
            arrayIndex++;
          }
        }
        return new string(array, 0, arrayIndex);
      }
      catch {
        return "";
      }

    }


    public static bool IsValidPhone(string Phone) {
      try {
        if (string.IsNullOrEmpty(Phone))
          return false;
        var r = new Regex(@"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$");
        return r.IsMatch(Phone);

      }
      catch (Exception) {
        throw;
      }
    }



    public static List<string> getPhoneHTML(List<string> datahtml, Dictionary<string, int> dauso) {
      try {
        //http://stackoverflow.com/questions/8596088/c-sharp-regex-phone-number-check
        //http://www.taphuan.vn/2015/05/xu-ly-chuoi-voi-regular-expression.html
        List<string> phoneList = new List<string>();
        List<string> phoneChuan = new List<string>();

        for (int i = 0; i < datahtml.Count; i++) {
          datahtml[i] = datahtml[i].Replace("+84", "0").Replace("(84)", "0").Replace(" ", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace("-", "");
        }
        List<string> result = datahtml.Where(x => dauso.Keys.Any(y => x.Contains(y))).ToList();
        foreach (var item in result) {
          foreach (regexs re in _regexs) {
            Regex rg = new Regex(string.Format(@"{0}", re.Regex));
            MatchCollection m = rg.Matches(item);
            foreach (Match g in m) {
              if (g.Groups[0].Value.Length > 0)
                if (phoneList.Count(x => x.Contains(g.Groups[0].Value)) == 0)
                  phoneList.Add(g.Groups[0].Value);
            }
          }

        }
        /*xu ly so lieu*/
        foreach (var item in phoneList) {
          string dienthoai = item.Replace(" ", "").Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "");

          Dictionary<string, int> dausothieu0 = dauso.Where(p => !p.Key.StartsWith("0")).ToDictionary(p => p.Key, p => p.Value);
          bool kiemtra0 = dausothieu0.Where(x => (dienthoai.StartsWith(x.Key) && dienthoai.Length == x.Value)).Count() > 0;
          if (kiemtra0)
            dienthoai = string.Format("0{0}", dienthoai);

          if (dienthoai.Length >= 10 && dienthoai.Length <= 11) {
            bool kiemtra = dauso.Where(x => (dienthoai.StartsWith(x.Key) && dienthoai.Length == x.Value)).Count() > 0;
            if (kiemtra)
              phoneChuan.Add(dienthoai);
          }
        }
        return phoneChuan.Distinct().ToList();

      }
      catch (Exception ex) {
        return null;

      }
    }

    public static List<string> getPhoneHTML1(List<string> datahtml, Dictionary<string, int> dauso, List<regexs> _regexs) {
      try {
        List<string> phoneList = new List<string>();
        List<string> phoneChuan = new List<string>();
        /*buoc 1: chỉnh lai trường hôp +84; 8*/
        for (int i = 0; i < datahtml.Count; i++)
          datahtml[i] = datahtml[i].Replace("+84", "0").Replace("(84)", "0");
        foreach (var item in datahtml) {
          foreach (regexs re in _regexs) {
            Regex rg = new Regex(string.Format(@"{0}", re.Regex));
            MatchCollection m = rg.Matches(item);
            foreach (Match g in m) {
              if (g.Groups[0].Value.Length > 0)
                if (phoneList.Count(x => x.Contains(g.Groups[0].Value)) == 0)
                  phoneList.Add(g.Groups[0].Value);
            }
          }
        }
        /*xu ly so lieu*/
        foreach (var item in phoneList) {
          string dienthoai = item.Replace(" ", "").Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "");
          Dictionary<string, int> dausothieu0 = dauso.Where(p => !p.Key.StartsWith("0")).ToDictionary(p => p.Key, p => p.Value);
          bool kiemtra0 = dausothieu0.Where(x => (dienthoai.StartsWith(x.Key) && dienthoai.Length == x.Value)).Count() > 0;
          if (kiemtra0)
            dienthoai = string.Format("0{0}", dienthoai);

          if (dienthoai.Length >= 10 && dienthoai.Length <= 11) {
            bool kiemtra = dauso.Where(x => (dienthoai.StartsWith(x.Key) && dienthoai.Length == x.Value)).Count() > 0;
            if (kiemtra)
              phoneChuan.Add(dienthoai);
          }
        }
        return phoneChuan.Distinct().ToList();

      }
      catch (Exception ex) {
        return null;

      }
    }

    public static List<string> getPhoneHTML(string content, Dictionary<string, int> dauso, List<regexs> _regexs, bool? istestTrung = null) {
      List<string> hashSet = new List<string>();
      List<string> phoneChuan = new List<string>();
      if (content == null || content == "") return null;
      content = content.Replace("(84)", "0").Replace("(+84)", "0").Replace("+84", "0");
      foreach (regexs re in _regexs) {
        Regex rg = new Regex(string.Format(@"{0}", re.Regex), RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
        MatchCollection m = rg.Matches(content);
        foreach (Match g in m) {
          string item = RemoveSymbolInPhone(g.Value);
          if (istestTrung == null || istestTrung == true) {
            if (!hashSet.Contains(item)) {
              hashSet.Add(item);
            }
          }
          else {
            hashSet.Add(item);
          }
        }
      }

      /*01/2018 bo xung theo kiem tra so luong chuoi dien thoai*/
      /*xu ly so lieu*/
      foreach (var item in hashSet) {
        string dienthoai = item;
        Dictionary<string, int> dausothieu0 = dauso.Where(p => !p.Key.StartsWith("0")).ToDictionary(p => p.Key, p => p.Value);
        bool kiemtra0 = dausothieu0.Where(x => (dienthoai.StartsWith(x.Key) && dienthoai.Length == x.Value)).Count() > 0;
        if (kiemtra0)
          dienthoai = string.Format("0{0}", dienthoai);

        if (dienthoai.Length >= 10 && dienthoai.Length <= 11) {
          bool kiemtra = dauso.Where(x => (dienthoai.StartsWith(x.Key) && dienthoai.Length == x.Value)).Count() > 0;
          if (kiemtra)
            phoneChuan.Add(dienthoai);
        }
      }
      return phoneChuan;
    }
    public static List<string> getPhoneHTML(List<string> datahtml, Dictionary<string, int> dauso, List<regexs> _regexs, bool? istestTrung = null) {
      List<string> hashSet = new List<string>();
      List<string> phoneChuan = new List<string>();
      foreach (var chuoi in datahtml) {
        string content = chuoi.Replace("(84)", "0").Replace("(+84)", "0").Replace("+84", "0");

        foreach (regexs re in _regexs) {
          Regex rg = new Regex(string.Format(@"{0}", re.Regex), RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
          MatchCollection m = rg.Matches(content);
          foreach (Match g in m) {
            string item = RemoveSymbolInPhone(g.Value);
            if (istestTrung == null || istestTrung == true) {
              if (!hashSet.Contains(item)) {
                hashSet.Add(item);
              }
            }
            else {
              hashSet.Add(item);
            }
          }
        }
      }
      /*01/2018 bo xung theo kiem tra so luong chuoi dien thoai*/
      /*xu ly so lieu*/
      foreach (var item in hashSet) {
        string dienthoai = item;
        Dictionary<string, int> dausothieu0 = dauso.Where(p => !p.Key.StartsWith("0")).ToDictionary(p => p.Key, p => p.Value);
        bool kiemtra0 = dausothieu0.Where(x => (dienthoai.StartsWith(x.Key) && dienthoai.Length == x.Value)).Count() > 0;
        if (kiemtra0)
          dienthoai = string.Format("0{0}", dienthoai);

        if (dienthoai.Length >= 10 && dienthoai.Length <= 11) {
          bool kiemtra = dauso.Where(x => (dienthoai.StartsWith(x.Key) && dienthoai.Length == x.Value)).Count() > 0;
          if (kiemtra)
            phoneChuan.Add(dienthoai);
        }
      }
      return phoneChuan;
    }

    public static string RemoveSymbolInPhone(string phonenum) {
      string text = phonenum;
      for (int i = 0; i < text.Length; i++) {
        char c = text[i];
        if (!char.IsDigit(c)) {
          phonenum = phonenum.Replace(c.ToString(), "");
        }
      }
      return phonenum;
    }


    public static List<string> getEmail(List<string> datahtml) {
      try {
        List<string> emailList = new List<string>();
        foreach (var item in datahtml) {
          Regex rg = new Regex(@"([\w\.])+@([a-zA-Z0-9\-])+\.([a-zA-Z]{2,4})(\.[a-zA-Z]{2,4})?");
          MatchCollection m = rg.Matches(item);
          foreach (Match g in m) {
            if (g.Groups[0].Value.Length > 0)
              if (emailList.Count(x => x.Contains(g.Groups[0].Value)) == 0)
                emailList.Add(g.Groups[0].Value);
          }
        }
        return emailList;
      }
      catch (Exception ex) {
        return null;

      }
    }

    private static List<string> CustomerLinkDomain(List<string> links) {
      List<string> ds = new List<string>();
      List<string> ds1 = new List<string>();

      try {

        foreach (var item in links) {
          string strkq = "";
          string item2 = item.Replace(" ", "").Replace("'", "");
          if (item2.StartsWith(_doman) || item2.StartsWith(_doman.Replace("www.", "")))
            strkq = item2;
          else if (item2.StartsWith("/"))
            strkq = string.Format("{0}{1}", _doman, item2);
          else if (!item2.StartsWith("https:") && !item.StartsWith("http:"))
            strkq = string.Format("{0}/{1}", _doman, item2);
          ds1.Add(strkq);

        }

        var grouped = ds1
            .GroupBy(s => s)
            .Select(group => new { Word = group.Key });

        foreach (string item in grouped.Select(x => x.Word).ToList()) {
          if (item != "")
            ds.Add(item);
        }
        return ds;
      }
      catch (Exception ex) {
        //MessageBox.Show(ex.Message, "CustomerLinkDomain");
        return ds;
      }
    }

    public static Task<string> stringWeb(string path, ref object arrControl, ref int solanlap) {
      HttpWebRequest req;

      ArrayList arr1 = (ArrayList)arrControl;
      Label lbl_message = (Label)arr1[0];
      Label lbl_scanner_khoa = (Label)arr1[1];
      Label lbl_handoi = (Label)arr1[2];
      Label lbl_sl_phone = (Label)arr1[3];
      Label lbl_sl_email = (Label)arr1[4];
      Label lbl_scance_sl_Link = (Label)arr1[5];

      try {

        req = (HttpWebRequest)WebRequest.Create(path);

        req.Credentials = CredentialCache.DefaultCredentials;
        req.AllowWriteStreamBuffering = false;
        req.Proxy = null;

        req.Timeout = _timeout;
        req.ReadWriteTimeout = _timeout;
        req.ContentType = "text/html";
        req.Method = WebRequestMethods.Http.Get;


        /*fix loi: The remote server returned an error: (403) Forbidden.*/
        req.UserAgent = _UserAgent;
        req.Accept = "*/*";

        //https://www.thomaslevesque.com/2014/11/04/passing-parameters-by-reference-to-an-asynchronous-method/
        var error = new Ref<string>();
        Task<WebResponse> task = MakeRequestAsync(req, error);
        return task.ContinueWith(t => ReadStreamFromResponse(t.Result, error.Value));

      }

      catch (Exception ex) {
        writer = LogWriter.Instance;
        writer.WriteToLog(string.Format("{0}-{1}-{2}", ex.ToString(), path, "stringWeb"));
        throw;
      }
    }

    private async static Task<WebResponse> MakeRequestAsync(HttpWebRequest req, Ref<string> err) {
      Task<WebResponse> task = Task.Factory.FromAsync<WebResponse>(req.BeginGetResponse, req.EndGetResponse, null);
      try {
        return await task;
      }
      catch (Exception ex) {
        err.Value = string.Format("phone_email_autoload_create_by_luulong {0}", ex.Message);
        return null;
      }
    }

    private static string ReadStreamFromResponse(WebResponse response, string err) {
      try {
        using (Stream responseStream = response.GetResponseStream())
        using (StreamReader sr = new StreamReader(responseStream)) {
          //Need to return this response 
          string strContent = sr.ReadToEnd();
          return strContent;
        }
      }
      catch (Exception ex) {
        return err;
      }

    }

    public static void getwebBrowserFindLink(dm_scanner_ct link, ref LinkQueues2 _queue, ref object arrControl, ref int solanlap) {
      try {
        string output = "";
        ArrayList arr1 = (ArrayList)arrControl;
        Label lbl_message = (Label)arr1[0];
        Label lbl_scanner_khoa = (Label)arr1[1];
        Label lbl_handoi = (Label)arr1[2];
        Label lbl_sl_phone = (Label)arr1[3];
        Label lbl_sl_email = (Label)arr1[4];
        Label lbl_scance_sl_Link = (Label)arr1[5];
        Label lblLienKetFind = (Label)arr1[6];
        if (!Utilities_scanner.hasProcess)
          return;/*stop*/

        if (_dosau != -1) {
          if (link.dosau >= _dosau)
            return;
        }
        output = stringWeb(link.path, ref arrControl, ref solanlap).Result;
        if (output.ToLower().Contains("phone_email_autoload_create_by_luulong")) {
          if (output.ToLower().Contains("time")) {
            if (solanlap <= _lanquetlai && _lanquetlai != -1) {
              solanlap = solanlap + 1;
              Lable(lbl_scanner_khoa, string.Format("Đứt Kết Nối: + {0}", solanlap.ToString()));
              getwebBrowserFindLink(link, ref _queue, ref arrControl, ref solanlap);
            }
            else if (_lanquetlai == -1) {
              solanlap = 0;
              lbl_scanner_khoa.Invoke((MethodInvoker)(() => lbl_scanner_khoa.Text = string.Format("Đứt Kết Nối.....")));
              getwebBrowserFindLink(link, ref _queue, ref arrControl, ref solanlap);
            }

          }
          else {    /*trường hop sai loi*/
                    //writer = LogWriter.Instance;
                    //writer.WriteToLog(string.Format("{0}-{1}-{2}", "khong co html1", link.path, "getwebBrowserFindLink"));
            lbl_message.BeginInvoke((MethodInvoker)delegate () {
              string chuoi = lbl_message.Text;
              if (Regex.Split(chuoi, Environment.NewLine).Count() == 30) {
                chuoi = "";
              }
              lbl_message.Text = chuoi + string.Format("- Lỗi: {0} {1} ", link.path.Replace("phone_email_autoload_create_by_luulong", " "), Environment.NewLine);
            });
            return;
          }
        }
        else {
          if (output.ToLower().Contains("sorry! something went wrong.")) {
            lbl_message.BeginInvoke((MethodInvoker)delegate () {
              string chuoi = lbl_message.Text;
              if (Regex.Split(chuoi, Environment.NewLine).Count() == 30) {
                chuoi = "";
              }
              lbl_message.Text = chuoi + string.Format("{0} {1} {0} {2}", Environment.NewLine, "Cảnh báo vui lòng chọn lại User Agent :", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36");
            });
            return;
          }
        }
        //sorry! something went wrong.
        lbl_scanner_khoa.Invoke((MethodInvoker)(() => lbl_scanner_khoa.Text = string.Format("Đang quét.....")));

        string title = Helpers.getTitleWeb(output.ToString());
        if (title == "") {  /*truong hop vat gia loi connect*/
          Lable(lbl_scanner_khoa, string.Format("Đứt Kết Nối...."));
          if (solanlap <= _lanquetlai) {
            solanlap = solanlap + 1;
            Thread.Sleep(_sleep);
            getwebBrowserFindLink(link, ref _queue, ref arrControl, ref solanlap);
          }
        }
        getPhone_Email(output.ToString(), ref arrControl, link);
        List<string> dsLinks = CustomerLinkDomain(Helpers.getUrlHtml2(output.ToString()));
        Lable(lblLienKetFind, string.Format("{0}", dsLinks.Count().ToString("#,#", CultureInfo.InvariantCulture)));
        dm_scanner_ct ls;
        foreach (var item in dsLinks) {
          if (!Utilities_scanner.hasProcess)
            break;
          ls = new dm_scanner_ct() {
            path = item,
            dosau = link.dosau + 1,
            statur = false,
            name = title,
            parentid = _queue.getIdLienKet()
          };

          if (_queue.EnqueueLinks1(ls)) {
            _tonglink++;

            if (_HienThiChiTietQuet)
              lbl_message.BeginInvoke((MethodInvoker)delegate () {
                string chuoi = lbl_message.Text;
                if (Regex.Split(chuoi, Environment.NewLine).Count() == 30) {
                  chuoi = "";
                }
                lbl_message.Text = chuoi + string.Format("- Độ sâu: {0} - liên kết: {1} {2}", ls.dosau, ls.path, Environment.NewLine);
              });
          }
          Lable(lbl_scance_sl_Link, string.Format("{0}", _tonglink.ToString("#,#", CultureInfo.InvariantCulture)));
        }
        Lable(lbl_handoi, string.Format("{0}", _queue.CountQueue1().ToString("#,#", CultureInfo.InvariantCulture)));

      }
      catch (Exception ex) {
        writer = LogWriter.Instance;
        writer.WriteToLog(string.Format("{0}-{1}-{2}", ex.Message, link.path, "getwebBrowserFindLink"));
        return;
      }
    }


    public static void Lable(Label lbl, string item) {
      if (lbl.InvokeRequired) {
        lbl.BeginInvoke(new MethodInvoker(() => Lable(lbl, item)));
      }
      else {
        lbl.Text = item;
        lbl.Update();
      }
    }

    public static void getPhone_Email(string strHTML, ref object arrControl, dm_scanner_ct model) {
      try {

        ArrayList arr1 = (ArrayList)arrControl;
        Label lbl_message = (Label)arr1[0];
        Label lbl_scanner_khoa = (Label)arr1[1];
        Label lbl_handoi = (Label)arr1[2];
        Label lbl_sl_phone = (Label)arr1[3];
        Label lbl_sl_email = (Label)arr1[4];

        List<string> ds = strHTML.Split('\r').ToList();

        if (_sodienthoai) {
          List<string> dsdienthoai = getPhoneHTML(ds, _dau_so);
          if (dsdienthoai != null)
            foreach (var item in dsdienthoai) {
              if (SQLDatabase.AddScanner_phone(new scanner_phone() { phone = item, dm_scanner_ct_id = model.id })) {
                _slPhone++;
              }
            }
          lbl_sl_phone.BeginInvoke((MethodInvoker)delegate () {
            lbl_sl_phone.Text = _slPhone.ToString("#,#", CultureInfo.InvariantCulture);
            lbl_sl_phone.Update();
          });
        }
        if (_emails) {
          List<string> emails = getEmail(ds);
          if (emails != null)
            foreach (var item in emails) {
              if (SQLDatabase.AddScanner_email(new scanner_email() { email = item, dm_scanner_ct_id = model.id })) {
                _slEmail++;
              }
            }
          lbl_sl_email.BeginInvoke((MethodInvoker)delegate () {
            lbl_sl_email.Text = _slEmail.ToString("#,#", CultureInfo.InvariantCulture);
            lbl_sl_email.Update();
          });
        }
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message, "getPhone_Email");
      }
    }
    public static void getwebBrowserPhone_Email(dm_scanner_ct links, ref LinkQueues queue, ref object arrControl, ref int solanquetlai) {
      HttpWebRequest req;
      HttpWebResponse resp;
      StreamReader reader;
      StringBuilder output;
      ArrayList arr1 = (ArrayList)arrControl;
      Label lbl_scanner_mess1 = (Label)arr1[0];
      Label lbl_scanner_mess2 = (Label)arr1[1];
      Label lbl_scanner_mess3 = (Label)arr1[2];

      try {
        if (!Utilities_scanner.hasProcess)
          return;/*stop*/

        //string xx = "http://vatgia.com/home/shop.php?view=list&iCat=433";
        req = (HttpWebRequest)WebRequest.Create(links.path);
        if (_timeout > 0) {
          req.Timeout = _timeout;
          req.ReadWriteTimeout = _timeout;
        }

        /*fix loi: The remote server returned an error: (403) Forbidden.*/
        req.UserAgent = "Foo";
        req.Accept = "*/*";
        Thread.Sleep(_sleep);

        resp = (HttpWebResponse)req.GetResponse();

        reader = new StreamReader(resp.GetResponseStream());
        output = new StringBuilder();

        output.Append(reader.ReadToEnd());

        lbl_scanner_mess1.Visible = false;
        lbl_scanner_mess1.Text = string.Format("{0}Đứt Kết Nối: {0}", Environment.NewLine);
        lbl_scanner_mess1.Update();

        string lochtml = StripTagsCharArray(output.ToString());
        List<string> ds1 = lochtml.Split('\r').ToList();
        string titleweb = Helpers.getTitleWeb(output.ToString());
        if (titleweb == "") {
          if (solanquetlai <= _lanquetlai) {
            solanquetlai = solanquetlai + 1;
            lbl_scanner_mess1.Visible = true;
            lbl_scanner_mess1.Update();
            Thread.Sleep(_sleep);
            getwebBrowserPhone_Email(links, ref queue, ref arrControl, ref solanquetlai);
          }
          else
            return;
        }

        if (_sodienthoai) {
          List<string> dsdienthoai = getPhoneHTML(ds1, _dau_so);
          if (dsdienthoai != null)
            foreach (var item in dsdienthoai) {
              if (SQLDatabase.AddScanner_phone(new scanner_phone() { phone = item, dm_scanner_ct_id = links.id })) {
                _slPhone++;
              }
            }
        }
        if (_emails) {
          List<string> emails = getEmail(ds1);
          if (emails != null)
            foreach (var item in emails) {
              if (SQLDatabase.AddScanner_email(new scanner_email() { email = item, dm_scanner_ct_id = links.id })) {
                _slEmail++;
              }
            }
        }
        lbl_scanner_mess2.Text = string.Format("Đã xử lý:{0}    - Email: {1} / Phone: {2}", queue.DaXuLy(), _slEmail, _slPhone);
        lbl_scanner_mess2.Update();

        lbl_scanner_mess3.Text = string.Format("Đang xử lý: Độ sâu {0} -   Liên kết: {1}", links.dosau, links.path);
        lbl_scanner_mess3.Update();


      }
      catch (WebException ex) {
        if (ex.ToString().Contains("time")) {
          if (solanquetlai <= _lanquetlai && _lanquetlai != -1) {
            solanquetlai = solanquetlai + 1;
            lbl_scanner_mess1.Visible = true;
            lbl_scanner_mess1.Text = string.Format("Đứt Kết Nối: + {0}", solanquetlai - 1);
            lbl_scanner_mess1.Update();
            Thread.Sleep(_sleep);
            getwebBrowserPhone_Email(links, ref queue, ref arrControl, ref solanquetlai);
          }
          else if (_lanquetlai == -1) {
            lbl_scanner_mess1.Visible = true;
            lbl_scanner_mess1.Text = string.Format("Đứt Kết Nối");
            lbl_scanner_mess1.Update();
            Thread.Sleep(_sleep);
            solanquetlai = 0;
            getwebBrowserPhone_Email(links, ref queue, ref arrControl, ref solanquetlai);
          }
        }
      }
      catch (Exception ex) {
        return;
      }
    }
  }
  public static class Utilities_trangvang {
    public static bool hasProcess = true; /*khai bao bien stop*/
    public static int _PathLimit;/*so trang quet gioi han*/
    public static int _lanquetlai;
    public static int _Sleep;
    public static int _Timeout;
    public static Dictionary<string, int> _dau_so;
    public static List<regexs> _regexs;
    public static bool ChkCKiemTraTrung = false;
    public static int IdDanhmuc;
    public static int _thanhcong = 0;
    public static int _thatbai = 0;
    //public static List<string> ds_dauso;

    private static LogWriter writer;

    public static string WebRequestNavigate(string url, Label lbl) {
      Stream stream = null;
      StringBuilder output = new StringBuilder();

      while (stream == null) {
        try {

          StreamReader reader;
          HttpWebResponse resp = null;
          HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
          resp = (HttpWebResponse)myReq.GetResponse();

          stream = resp.GetResponseStream();
          //stream.ReadTimeout = 3000;
          reader = new StreamReader(resp.GetResponseStream());
          output.Clear();
          output.Append(reader.ReadToEnd());

          if (output.Length <= 400) {

            if (lbl != null) {
              lbl.Visible = true;
              lbl.Update();
              if (lbl.Text.Length < 20) {
                lbl.Text = lbl.Text + "*";

              }
              else {
                lbl.Text = "Khoá IP";
              }
              lbl.Update();
            }
            System.Threading.Thread.Sleep(5000);
            stream = null;
          }
        }
        catch (Exception e) {
          if (lbl != null) {
            lbl.Visible = true;
            lbl.Update();
            if (lbl.Text.Length < 20) {
              lbl.Text = lbl.Text + "*";

            }
            else {
              lbl.Text = "Khoá IP";
            }
            lbl.Update();
          }
          System.Threading.Thread.Sleep(5000);
          stream = null;
        }

      }
      if (lbl != null) {
        lbl.Visible = false;
        lbl.Update();
      }

      return output.ToString();
    }
    private static int PathMax(string[] arr) {
      try {
        string[] page = Array.FindAll(arr, n => n.Contains("<a href=\"?page="));
        int i = 0;
        foreach (var item in page) {
          int so = ConvertType.ToInt(Helpers.getDataHTML(item.Trim().Replace(".", ""))[0]);
          i = i > so ? i : so;
        }
        return i;
      }
      catch (Exception ex) {
        return 0;
      }
    }
    public static int getPageMax(string url, Label lblkhoa) {
      string strHtml = "";
      string[] arrGetHtmlDocumentALL;
      try {
        strHtml = Utilities_trangvang.WebRequestNavigate(url, lblkhoa);
        arrGetHtmlDocumentALL = strHtml.Split('\n');

        string[] page = Array.FindAll(arrGetHtmlDocumentALL, n => n.Contains("<a href=\"?page="));
        int i = 0;
        foreach (var item in page) {
          int so = ConvertType.ToInt(Helpers.getDataHTML(item.Trim().Replace(".", ""))[0]);
          i = i > so ? i : so;
        }
        return i;

      }
      catch (Exception ex) {
        //MessageBox.Show(ex.Message, "getPageNext");
        return 0;
      }
    }
    public static void ShowMessage(Label lbl, string chitiet, int thanhcong, int thatbai) {
      lbl.Text = string.Format("Thành công: {0} /Thất bại: {1} -> {2}", thanhcong, thatbai, chitiet);
      lbl.Update();
    }
    public static void getwebBrowser(string strPath, ref int solanlap, Label lbl, Label lblkhoa) {

      HttpWebRequest req;
      HttpWebResponse resp;
      StreamReader reader;
      StringBuilder output;
      string[] arrGetHtmlDocumentALL;
      Dictionary<string, string> dictionary = new Dictionary<string, string>();

      try {

        if (!Utilities_trangvang.hasProcess)
          return;/*stop*/

        ServicePointManager.DefaultConnectionLimit = 100;
        req = (HttpWebRequest)WebRequest.Create(strPath);
        req.Proxy = null;
        req.Timeout = _Timeout;
        //req.ReadWriteTimeout = _Timeout;

        resp = (HttpWebResponse)req.GetResponse();

        reader = new StreamReader(resp.GetResponseStream());

        //if (resp.StatusCode == HttpStatusCode.OK) {
        //    //Perform necessary action based on response
        //}

        output = new StringBuilder();

        output.Append(reader.ReadToEnd());
        string title = Helpers.getTitleWeb(output.ToString());
        if (title == "") {  /*truong hop vat gia loi connect*/
          if (solanlap <= _lanquetlai) {
            solanlap = solanlap + 1;
            lblkhoa.Visible = true;
            lblkhoa.Update();
            Thread.Sleep(_Sleep);
            getwebBrowser(strPath, ref solanlap, lbl, lblkhoa);
          }
          else
            _thatbai++;
        }
        lblkhoa.Visible = false;
        lblkhoa.Update();

        arrGetHtmlDocumentALL = output.ToString().Split('\n');
        string[] arrcompany_nameUrl = Array.FindAll(arrGetHtmlDocumentALL, c => c.Contains("company_name"));

        trangvang tv;
        foreach (var item in arrcompany_nameUrl) {
          tv = new trangvang();
          string strUrlChiTiet = Helpers.getUrlHtml(item).Replace(" ", "").Replace("\t", "");   /*xu ly linh chi tiet loi sai link \t*/

          int solanlap1 = 0;
          getTrangCon(strUrlChiTiet, ref tv, ref solanlap1, lblkhoa);

          if (Utilities_trangvang.hasProcess)/*nếu đang chạy mới được phép lưu thông tin, còn đã dừng tiến trình thi không được phép lưu*/
            if (SQLDatabase.AddTrangVang(tv))   /*lưu thông tin vào csdl*/
              _thanhcong++;
            else
              _thatbai++;
          ShowMessage(lbl, strUrlChiTiet, _thanhcong, _thatbai);
        }

      }
      catch (WebException ex) {
        if (ex.ToString().Contains("time")) {
          if (solanlap <= _lanquetlai && _lanquetlai != -1) {
            solanlap = solanlap + 1;

            lblkhoa.Visible = true;
            lblkhoa.Text = string.Format("Đứt Kết Nối : + {0}", solanlap - 1);
            lblkhoa.Update();

            Thread.Sleep(_Sleep);
            getwebBrowser(strPath, ref solanlap, lbl, lblkhoa);
          }
          else if (_lanquetlai == -1) {
            lblkhoa.Visible = true;
            lblkhoa.Text = string.Format("Đứt Kết Nối..");
            lblkhoa.Update();
            solanlap = 0;
            Thread.Sleep(_Sleep);
            getwebBrowser(strPath, ref solanlap, lbl, lblkhoa);
          }
          else
            _thatbai++;
        }
      }
      catch (Exception e) {
        return;
      }
    }


    public static void getDanhMuc(string strPath, ref Dictionary<string, string> dm) {

      string[] arrGetHtmlDocumentALL;
      Dictionary<string, string> dictionary = new Dictionary<string, string>();

      try {

        string strHtml = Utilities_trangvang.WebRequestNavigate(strPath, null);
        arrGetHtmlDocumentALL = strHtml.Split('\n');

        string[] arrDanhSach = Array.FindAll(arrGetHtmlDocumentALL, c => c.Contains("<p class=\"pcategory\""));

        foreach (var item in arrDanhSach) {
          if (!item.Contains("Ngành nghề bắt đầu bằng chữ cái") &&
              !item.Contains("font-size:11px;")) {
            string strdk = "<a";
            int vt1 = item.IndexOf(strdk);
            int vt2 = item.IndexOf("</a>", vt1);
            string chuoi = item.Substring(vt1, vt2 - vt1) + "</a>";

            string strurl = Helpers.getUrlHtml(chuoi);
            string strValue = Helpers.getDataHTML(chuoi).FirstOrDefault();
            if (!dm.ContainsKey(strurl))
              dm.Add(Helpers.getUrlHtml(chuoi), Helpers.getDataHTML(chuoi).FirstOrDefault());
          }
        }


      }
      catch (Exception ex) {
        writer = LogWriter.Instance;
        writer.WriteToLog(string.Format("{0}-{1}-{2}", ex.Message, "getDanhMuc", strPath));
        MessageBox.Show(ex.Message, "getDanhMuc");
      }
    }
    public static void getTrangCon(string strPath, ref trangvang tv, ref int solanlap, Label lblkhoa) {
      HttpWebRequest req;
      //HttpWebResponse resp;
      StreamReader reader;
      StringBuilder output;

      List<string> arrGetHtmlDocumentALL;
      List<string> arrGetHtmlDocumentALLLienHe;
      int vt = -1;

      try {
        if (!Utilities_trangvang.hasProcess)
          return;/*stop*/

        //strPath = "http://trangvangvietnam.com/listings/716349/mhe-demag_viet_nam_cong_ty_tnhh_mhe-demag_viet_nam.html";
        ServicePointManager.DefaultConnectionLimit = 100;
        req = (HttpWebRequest)WebRequest.Create(strPath);
        req.Proxy = null;
        req.Timeout = _Timeout;
        //req.ReadWriteTimeout = _Timeout;

        //resp = (HttpWebResponse)req.GetResponse();
        using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse()) {
          // Do your processings here....
          reader = new StreamReader(resp.GetResponseStream());
          output = new StringBuilder();

          output.Append(reader.ReadToEnd());
          string strWebsite = output.ToString();
          string title = Helpers.getTitleWeb(strWebsite);
          if (title == "") {  /*truong hop vat gia loi connect*/
            if (solanlap <= _lanquetlai) {
              solanlap = solanlap + 1;
              lblkhoa.Visible = true;
              lblkhoa.Update();
              Thread.Sleep(_Sleep);
              getTrangCon(strPath, ref tv, ref solanlap, lblkhoa);
            }
            else
              _thatbai++;
          }

          lblkhoa.Visible = false;
          lblkhoa.Update();

          int vt1 = strWebsite.IndexOf("div id=\"listing_detail_left\">");
          int vt2 = strWebsite.IndexOf("<div id=\"listing_detail_right\">", vt1);
          string chuoixuly = strWebsite.Substring(vt1, vt2 - vt1);

          arrGetHtmlDocumentALL = chuoixuly.Split(new char[] { '\n', '\r' }).ToList();

          /*sys_website*/
          tv.sys_diachiweb = strPath;
          tv.danhmucid = IdDanhmuc;
          /*detailcompany_name- Tên công ty*/
          //vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Tên công ty:", "<div class=\"tuadehoso\">", true);
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<div class=\"tencongty\">");
          if (vt != -1) {
            tv.ten_cong_ty = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt].ToString()).FirstOrDefault();
          }
          /*Địa chỉ:*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<div class=\"diachi_chitiet_li2dc\">");
          if (vt != -1) {
            string duong_phuong_huyen = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 2].ToString()).FirstOrDefault().Trim();
            string tinh = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 2].ToString())[1].Trim().Replace(",", "");
            string quocgia = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 2].ToString()).LastOrDefault().Trim().Replace(",", "");
            if (duong_phuong_huyen.LastIndexOf(",") != duong_phuong_huyen.Length - 1)
              tv.dia_chi = duong_phuong_huyen + "," + tinh + "," + quocgia;
            else
              tv.dia_chi = duong_phuong_huyen + tinh + "," + quocgia;
          }
          /*Điện thoại:*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Tel", "<div class=\"diachi_chitiet_li1\">", true);
          if (vt != -1) {
            tv.dien_thoai = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 2].ToString()).FirstOrDefault();
            List<string> arrDienThoai = tv.dien_thoai.Split(',').ToList();
            if (arrDienThoai.Count() > 0) {
              List<string> dsdienthoai = Utilities_scanner.getPhoneHTML(arrDienThoai, _dau_so, _regexs);
              tv.di_dong = dsdienthoai.Count() == 0 ? "" : dsdienthoai.FirstOrDefault();
            }
          }
          /*Fax:*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Fax:", "<div class=\"diachi_chitiet_li1\">", true);
          if (vt != -1) {
            tv.fax = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 2].ToString()).FirstOrDefault();
          }
          /*Email:*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "email_icon_yellow.png");
          if (vt != -1) {
            tv.email_cty = Utilities_scanner.getEmail(Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 2].ToString())).FirstOrDefault();
          }
          //icon_email_website
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "website_icon_yellow.png");
          if (vt != -1) {
            tv.website = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 2].ToString()).FirstOrDefault();
          }
          /*Loại hình:*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Loại hình công ty:", "<div class=\"thitruong_loaidn_title_txt\">", true);
          if (vt != -1) {
            tv.loai_hinh = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 4].ToString()).FirstOrDefault();
          }
          /*Thị trường:*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Thị trường chính:", "<div class=\"thitruong_loaidn_title_txt\">", true);
          if (vt != -1) {
            tv.thi_truong = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 4].ToString()).FirstOrDefault();
          }
          /*Mã số thuế:*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Mã số thuế:", "class=\"hosocongty_tite_text\"", true);
          if (vt != -1) {
            tv.msthue = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 4].ToString()).FirstOrDefault();
          }
          /*Năm thành lập:*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Năm thành lập:", "class=\"hosocongty_tite_text\"", true);
          if (vt != -1) {
            tv.nam_thanh_lap = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 4].ToString()).FirstOrDefault();
          }
          /*Số nhân viên:*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Số lượng nhân viên:", "class=\"hosocongty_tite_text\"", true);
          if (vt != -1) {
            tv.so_nhan_vien = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 4].ToString()).FirstOrDefault();
          }
          /*Chứng nhận*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Chứng nhận:", "<div class=\"hosocongty_tite_text\">", true);
          if (vt != -1) {
            tv.chung_nhan = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 4].ToString()).FirstOrDefault();
          }

          /*=============THÔNG TIN LIÊN HỆ*/
          int vt_lienhe1 = strWebsite.IndexOf("<div class=\"lienhe_trangchitiet\">");
          if (vt_lienhe1 != -1) {
            int vt_lienhe2 = strWebsite.IndexOf("<div class=\"banladoanhnghiep\">", vt_lienhe1);
            string chuoixuly_lienhe = strWebsite.Substring(vt_lienhe1, vt_lienhe2 - vt_lienhe1);

            arrGetHtmlDocumentALLLienHe = chuoixuly_lienhe.Split(new char[] { '\n', '\r' }).ToList();
            /*người liên hệ*/
            int vitriranhgioi = -1;
            vitriranhgioi = Helpers.vitritim(arrGetHtmlDocumentALLLienHe, "<div class=\"ngan_dong_lienhe\"></div>");
            if (vitriranhgioi == -1) {
              vt = Helpers.vitritim(arrGetHtmlDocumentALLLienHe, "Người liên hệ:", "<div class=\"dong_lienhe_tuade\">", true);
              if (vt != -1) {
                tv.nguoi_lien_he1 = Helpers.getDataHTML(arrGetHtmlDocumentALLLienHe[vt + 2].ToString()).LastOrDefault();
              }
              vt = Helpers.vitritim(arrGetHtmlDocumentALLLienHe, "Chức vụ:", "<div class=\"dong_lienhe_tuade\">", true);
              if (vt != -1) {
                tv.chuc_vu1 = Helpers.getDataHTML(arrGetHtmlDocumentALLLienHe[vt + 2].ToString()).LastOrDefault();
              }
              vt = Helpers.vitritim(arrGetHtmlDocumentALLLienHe, "Điện thoại:", "<div class=\"dong_lienhe_tuade\">", true);
              if (vt != -1) {
                tv.dien_thoai1 = Helpers.getDataHTML(arrGetHtmlDocumentALLLienHe[vt + 2].ToString()).LastOrDefault();
              }
              vt = Helpers.vitritim(arrGetHtmlDocumentALLLienHe, "Di động:", "<div class=\"dong_lienhe_tuade\">", true);
              if (vt != -1) {
                List<string> dsdienthoai = Utilities_scanner.getPhoneHTML(Helpers.getDataHTML(arrGetHtmlDocumentALLLienHe[vt + 2].ToString()), _dau_so, _regexs);
                tv.di_dong1 = dsdienthoai.FirstOrDefault();
              }
              vt = Helpers.vitritim(arrGetHtmlDocumentALLLienHe, "Email:", "<div class=\"dong_lienhe_tuade\">", true);
              if (vt != -1) {
                tv.email1 = Utilities_scanner.getEmail(Helpers.getDataHTML(arrGetHtmlDocumentALLLienHe[vt + 2])).FirstOrDefault();
              }
              vt = Helpers.vitritim(arrGetHtmlDocumentALLLienHe, "<img src=\"http://trangvangvietnam.com/images/skype-icon.png\">");
              if (vt != -1) {
                tv.Skype1 = Helpers.getUrlHtml(arrGetHtmlDocumentALLLienHe[vt]).Replace("skype:", "").Replace("?chat", "");
              }
            }
            else {

              vt = Helpers.vitritim(arrGetHtmlDocumentALLLienHe, "Người liên hệ:", "<div class=\"dong_lienhe_tuade\">", true);
              if (vt != -1) {
                tv.nguoi_lien_he1 = Helpers.getDataHTML(arrGetHtmlDocumentALLLienHe[vt + 2].ToString()).LastOrDefault();
              }
              vt = Helpers.vitritim(arrGetHtmlDocumentALLLienHe, "Chức vụ:", "<div class=\"dong_lienhe_tuade\">", true);
              if (vt != -1) {
                tv.chuc_vu1 = Helpers.getDataHTML(arrGetHtmlDocumentALLLienHe[vt + 2].ToString()).LastOrDefault();
              }
              vt = Helpers.vitritim(arrGetHtmlDocumentALLLienHe, "Điện thoại:", "<div class=\"dong_lienhe_tuade\">", true);
              if (vt != -1) {
                tv.dien_thoai1 = Helpers.getDataHTML(arrGetHtmlDocumentALLLienHe[vt + 2].ToString()).LastOrDefault();
              }
              vt = Helpers.vitritim(arrGetHtmlDocumentALLLienHe, "Di động:", "<div class=\"dong_lienhe_tuade\">", true);
              if (vt != -1) {
                List<string> dsdienthoai = Utilities_scanner.getPhoneHTML(Helpers.getDataHTML(arrGetHtmlDocumentALLLienHe[vt + 2].ToString()), _dau_so, _regexs);
                tv.di_dong1 = dsdienthoai.FirstOrDefault();
              }
              vt = Helpers.vitritim(arrGetHtmlDocumentALLLienHe, "Email:", "<div class=\"dong_lienhe_tuade\">", true);
              if (vt != -1) {
                tv.email1 = Utilities_scanner.getEmail(Helpers.getDataHTML(arrGetHtmlDocumentALLLienHe[vt + 2])).FirstOrDefault();
              }
              vt = Helpers.vitritim(arrGetHtmlDocumentALLLienHe, "<img src=\"http://trangvangvietnam.com/images/skype-icon.png\">");
              if (vt != -1) {
                //skype:vuong.tnt?chat
                tv.Skype1 = Helpers.getUrlHtml(arrGetHtmlDocumentALLLienHe[vt]).Replace("skype:", "").Replace("?chat", "");
              }

              for (int i = 0; i < vitriranhgioi; i++)    /*bo qua các dòng đã tìm, tìm đến dòng kế tiếp*/
                arrGetHtmlDocumentALLLienHe[i] = "";


              vt = Helpers.vitritim(arrGetHtmlDocumentALLLienHe, "Người liên hệ:", "<div class=\"dong_lienhe_tuade\">", true);
              if (vt != -1) {
                tv.nguoi_lien_he2 = Helpers.getDataHTML(arrGetHtmlDocumentALLLienHe[vt + 2].ToString()).LastOrDefault();
              }
              vt = Helpers.vitritim(arrGetHtmlDocumentALLLienHe, "Chức vụ:", "<div class=\"dong_lienhe_tuade\">", true);
              if (vt != -1) {
                tv.chuc_vu2 = Helpers.getDataHTML(arrGetHtmlDocumentALLLienHe[vt + 2].ToString()).LastOrDefault();
              }
              vt = Helpers.vitritim(arrGetHtmlDocumentALLLienHe, "Điện thoại:", "<div class=\"dong_lienhe_tuade\">", true);
              if (vt != -1) {
                tv.dien_thoai2 = Helpers.getDataHTML(arrGetHtmlDocumentALLLienHe[vt + 2].ToString()).LastOrDefault();
              }
              vt = Helpers.vitritim(arrGetHtmlDocumentALLLienHe, "Di động:", "<div class=\"dong_lienhe_tuade\">", true);
              if (vt != -1) {
                List<string> dsdienthoai = Utilities_scanner.getPhoneHTML(Helpers.getDataHTML(arrGetHtmlDocumentALLLienHe[vt + 2].ToString()), _dau_so, _regexs);
                tv.di_dong2 = dsdienthoai.FirstOrDefault();
              }
              vt = Helpers.vitritim(arrGetHtmlDocumentALLLienHe, "Email:", "<div class=\"dong_lienhe_tuade\">", true);
              if (vt != -1) {
                tv.email2 = Utilities_scanner.getEmail(Helpers.getDataHTML(arrGetHtmlDocumentALLLienHe[vt + 2])).FirstOrDefault();
              }
              vt = Helpers.vitritim(arrGetHtmlDocumentALLLienHe, "<img src=\"http://trangvangvietnam.com/images/skype-icon.png\">");
              if (vt != -1) {
                //skype:vuong.tnt?chat
                tv.Skype2 = Helpers.getUrlHtml(arrGetHtmlDocumentALLLienHe[vt]).Replace("skype:", "").Replace("?chat", "");
              }
            }
          }
        }

      }
      catch (WebException ex) {
        if (ex.ToString().Contains("time")) {
          if (solanlap <= _lanquetlai && _lanquetlai != -1) {
            solanlap = solanlap + 1;
            lblkhoa.Visible = true;
            lblkhoa.Text = string.Format("Đứt Kết Nối : + {0}", solanlap - 1);
            lblkhoa.Update();
            Thread.Sleep(_Sleep);
            getTrangCon(strPath, ref tv, ref solanlap, lblkhoa);
          }
          else if (_lanquetlai == -1) {
            lblkhoa.Visible = true;
            lblkhoa.Text = string.Format("Đứt Kết Nối");
            lblkhoa.Update();
            solanlap = 0;
            Thread.Sleep(_Sleep);
            getTrangCon(strPath, ref tv, ref solanlap, lblkhoa);
          }
          else
            _thatbai++;
        }
      }
      catch (Exception e) {
        _thatbai++;
        writer = LogWriter.Instance;
        writer.WriteToLog(string.Format("{0}-{1}-{2}", e.Message, strPath, "getTrangCon-trangvang"));
        return;
      }
    }
  }
  public static class Utilities_muaban {
    public static bool hasProcess = true; /*khai bao bien stop*/

    public static int _PathLimit;/*so trang quet gioi han*/
    public static int _lanquetlai;
    public static int _Sleep;
    public static int _Timeout;
    public static List<regexs> _regexs;

    public static int _IdDanhmuc;
    public static int _thanhcong = 0;
    public static int _thatbai = 0;
    public static Dictionary<string, int> _dau_so;
    public static infoPathmuaban _modelTrang;

    private static LogWriter writer;


    public static Dictionary<string, string> getDanhMuc(string strPath, Label lbl_khoa) {

      string[] arrGetHtmlDocumentALL;
      Dictionary<string, string> dictionary = new Dictionary<string, string>();

      try {
        int solanlap = 0;
        string strHtml = Utilities_muaban.WebRequestNavigate(strPath, ref solanlap, lbl_khoa).ToString();
        int vt1 = strHtml.IndexOf("nav mbn-list-nav col3");
        int vt2 = strHtml.IndexOf("</ul>", vt1);
        string chuoixuly = strHtml.Substring(vt1, vt2 - vt1);

        arrGetHtmlDocumentALL = chuoixuly.Split(new char[] { '\n', '\r' });

        string[] arrDanhSach = Array.FindAll(arrGetHtmlDocumentALL, c => c.Contains("href="));

        foreach (var item in arrDanhSach) {
          dictionary.Add(Helpers.getUrlHtml(item), Helpers.getDataHTML(item).FirstOrDefault());
        }
        return dictionary;
      }
      catch (Exception ex) {
        writer = LogWriter.Instance;
        writer.WriteToLog(string.Format("{0}-{1}-{2}", ex.Message, "getDanhMuc", strPath));
        MessageBox.Show(ex.Message, "getDanhMuc");
        return null;
      }
    }

    public static StringBuilder WebRequestNavigate(string url, ref int solanlap, Label lbl_khoa) {
      Stream stream = null;
      StringBuilder output = new StringBuilder();

      while (stream == null) {
        try {
          StreamReader reader;
          HttpWebResponse resp = null;
          HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
          resp = (HttpWebResponse)myReq.GetResponse();

          stream = resp.GetResponseStream();
          stream.ReadTimeout = _Timeout;
          reader = new StreamReader(resp.GetResponseStream());
          output.Clear();
          output.Append(reader.ReadToEnd());
        }
        catch (WebException ex) {
          if (ex.ToString().Contains("time")) {
            if (solanlap <= _lanquetlai && _lanquetlai != -1) {
              solanlap = solanlap + 1;

              lbl_khoa.Visible = true;
              lbl_khoa.Text = string.Format("Đứt Kết Nối : + {0}", solanlap - 1);
              lbl_khoa.Update();

              Thread.Sleep(_Sleep);
              WebRequestNavigate(url, ref solanlap, lbl_khoa);
            }
            else if (_lanquetlai == -1) {
              lbl_khoa.Visible = true;
              lbl_khoa.Text = string.Format("Đứt Kết Nối..");
              lbl_khoa.Update();
              solanlap = 0;
              Thread.Sleep(_Sleep);
              WebRequestNavigate(url, ref solanlap, lbl_khoa);
            }
            else
              _thatbai++;
          }
          //return null;
        }
        catch (Exception e) {
          return null;
        }
        return output;
      }
      return null;
    }

    public static void getPageInfo(string url, Label lblkhoa, ref infoPathmuaban model) {
      string str = "";
      try {
        int solanlap = 0;
        str = Utilities_muaban.WebRequestNavigate(url, ref solanlap, lblkhoa).ToString();
        if (str == "" || str == null) return;

        string[] arrGetHtmlDocumentALL = str.ToString().Split('\n');
        string[] arrcompany_nameUrl = Array.FindAll(arrGetHtmlDocumentALL, c => c.Contains("mbnSetting"));

        string pagesize = Array.FindAll(arrcompany_nameUrl, c => c.Contains("PageSize")).FirstOrDefault();
        MatchCollection matches = Regex.Matches(pagesize, "[0-9]+");
        model.PageSize = Array.FindAll(arrcompany_nameUrl, c => c.Contains("PageSize")) == null ? 0 : getMaxValue(matches);

        string totalPagingMax = Array.FindAll(arrcompany_nameUrl, c => c.Contains("TotalPagingMax")).FirstOrDefault();
        MatchCollection matches1 = Regex.Matches(totalPagingMax, "[0-9]+");
        model.TotalPagingMax = Array.FindAll(arrcompany_nameUrl, c => c.Contains("TotalPagingMax")) == null ? 0 : getMaxValue(matches1);

        string totalResult = Array.FindAll(arrcompany_nameUrl, c => c.Contains("TotalResult")).FirstOrDefault();
        MatchCollection matches2 = Regex.Matches(totalResult, "[0-9]+");
        model.TotalResult = Array.FindAll(arrcompany_nameUrl, c => c.Contains("TotalResult")) == null ? 0 : getMaxValue(matches2);

      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message, "getPageInfo");
      }
    }

    public static int getMaxValue(MatchCollection model) {
      int i = 0;
      foreach (Match item in model) {
        if (i < ConvertType.ToInt(item.Value))
          i = ConvertType.ToInt(item.Value);
      }
      return i;
    }
    public static void getwebBrowser(string strPath, ref int solanlap, object arrControl) {
      ArrayList arr1 = (ArrayList)arrControl;
      Label lbl_muaban_message1 = (Label)arr1[0];
      Label lbl_muaban_message2 = (Label)arr1[1];
      Label lbl_muaban_khoa = (Label)arr1[2];
      TextBox txt_muaban_tv_link = (TextBox)arr1[3];
      ProgressBar pro_muaban_tv = (ProgressBar)arr1[4];
      Label lbl_par = (Label)arr1[5];

      StringBuilder output;
      List<string> arrGetHtmlDocumentALL;
      Dictionary<string, string> dictionary = new Dictionary<string, string>();

      try {

        if (!Utilities_muaban.hasProcess)
          return;/*stop*/

        // int solanlap2 = 0;
        output = new StringBuilder();
        output = WebRequestNavigate(strPath, ref solanlap, lbl_muaban_khoa);
        if (output == null) return;


        lbl_muaban_khoa.Visible = false;
        lbl_muaban_khoa.Update();

        string chuoimoi1 = "<div class=\"mbn-box-list\">";
        int vt1 = output.ToString().IndexOf(chuoimoi1);
        string str1 = output.ToString().Substring(vt1, output.ToString().Length - vt1);
        int vt2 = str1.IndexOf("<div class=\"paging\">");
        string str2 = str1.Substring(0, vt2 + chuoimoi1.Length);

        string chuoimoi2 = "<div class=\"mbn-box-list-content\">";
        string[] arrGetDocument = Array.FindAll(str2.Split(new string[] { chuoimoi2 }, StringSplitOptions.None), n => !n.Contains("<div class=\"mbn-box-list\">"));

        muaban timviec;
        int vt = -1;
        foreach (var item in arrGetDocument) {
          timviec = new muaban();
          timviec.danhmucid = _IdDanhmuc;

          string strUrlChiTiet = Helpers.getUrlHtml(item).Replace(" ", "").Replace("\t", "");   /*xu ly linh chi tiet loi sai link \t*/

          arrGetHtmlDocumentALL = item.Split(new char[] { '\n', '\r' }).ToList();

          /*đối tượng khách hàng*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "mbn-label");
          if (vt != -1)
            timviec.doituongkh = Helpers.getClassHtml(arrGetHtmlDocumentALL[vt].ToString()).Replace("mbn-label-", "");
          /*tiêu đề*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "mbn-title");
          if (vt != -1)
            timviec.tieude = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt].ToString()).FirstOrDefault();
          /*khuvuc*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "mbn-address");
          if (vt != -1) {
            timviec.khuvuc = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt].ToString()).FirstOrDefault();
            string[] strkhuvuc = timviec.khuvuc.Split(',');
            if (strkhuvuc.Count() == 1) {
              timviec.thanhpho = strkhuvuc[0].Trim();
            }
            else if (strkhuvuc.Count() == 2) {
              timviec.quan = strkhuvuc[0].Trim();
              timviec.thanhpho = strkhuvuc[1].Trim();
            }
          }
          /*mbn-date*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "mbn-date");
          if (vt != -1)
            timviec.ngaydang = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt].ToString()).FirstOrDefault();
          /*mbn-price*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "mbn-price");
          if (vt != -1)
            timviec.mucluong = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt].ToString()).FirstOrDefault().Replace("&nbsp;", "");


          //int solanlap1 = 0;
          getTrangCon(strUrlChiTiet, ref timviec, ref solanlap, arrControl);

          if (Utilities_muaban.hasProcess)
            if (timviec != null && SQLDatabase.AddMuaban_timviec(timviec))   /*lưu thông tin vào csdl*/
              _thanhcong++;
            else
              _thatbai++;
        }
      }
      catch (Exception e) {
        return;
      }
    }

    public static void ShowMessage(Label lbl_message_muaban_tv_2, string trang, string chitiet, int thanhcong, int thatbai, int vt) {
      lbl_message_muaban_tv_2.Text = string.Format("Ngành : {0} \nĐã quét:  -->{1}<--[{2}]  .Thành công: {3}/Thất bại:{4} \nLink: {5}", trang, vt, String.Format("{0:#,##0.##}", _modelTrang.TotalResult), thanhcong, thatbai, chitiet);
      lbl_message_muaban_tv_2.Update();
    }

    public static void getTrangCon(string strPath, ref muaban vg, ref int solanlap, object arrControl) {
      ArrayList arr1 = (ArrayList)arrControl;
      Label lbl_muaban_message1 = (Label)arr1[0];
      Label lbl_muaban_message2 = (Label)arr1[1];
      Label lbl_muaban_khoa = (Label)arr1[2];
      TextBox txt_muaban_tv_link = (TextBox)arr1[3];
      ProgressBar pro_muaban_tv = (ProgressBar)arr1[4];


      StringBuilder output;
      string strWebsite;

      try {
        //strPath = "https://vinabiz.org//company/detail/cong-ty-tnhh-trong-trot-cong-nghe-cao-dtk/3200360030003000390038003300320039003900";
        if (!Utilities_muaban.hasProcess)
          return;/*stop*/

        int solanlap1 = 0;
        output = new StringBuilder();
        output = WebRequestNavigate(strPath, ref solanlap1, lbl_muaban_khoa);
        if (output == null) return;

        strWebsite = output.ToString();

        /**************************Lấy danh mục nghề nghiệp***********************************/
        string strd1 = "class=\"breadcrumb breadcrumbs\"";
        string strd2 = "class=\"cl-detail clearfix\"";
        int vt11 = strWebsite.IndexOf(strd1);
        int vt12 = strWebsite.IndexOf(strd2, vt11);

        string strnoititel = strWebsite.Substring(vt11, vt12 - vt11);
        List<string> arrGetHtmlDocumentALL2 = strnoititel.Split(new char[] { '\n', '\r' }).Where(r => r.Contains("rel=\"v:url\" property=\"v:title\"")).ToList();
        vg.danhmucnghe = Helpers.getDataHTML(arrGetHtmlDocumentALL2[arrGetHtmlDocumentALL2.Count() - 1]).FirstOrDefault();

        vg.sys_diachiweb = strPath;
        /**************************************************************/
        string strdiemmoc1 = "id=\"dvContent\"";
        string strdiemmoc2 = "class=\"dt-service-ads\"";
        int vt1 = strWebsite.IndexOf(strdiemmoc1);
        int vt2 = strWebsite.IndexOf(strdiemmoc2, vt1);

        string strnoidung = strWebsite.Substring(vt1, vt2 - vt1);
        int vt = -1;
        List<string> arrGetHtmlDocumentALL = strnoidung.Split(new char[] { '\n', '\r' }).ToList();
        /*đối tượng khách hàng*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "icon-tag31");
        if (vt != -1) {
          vg.idtin = Regex.Match(Helpers.getDataHTML(arrGetHtmlDocumentALL[vt].ToString()).FirstOrDefault(), "[0-9]+").ToString();

        }
        /*da xác thực*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "icon-check33", "Đã xác thực", true);
        if (vt != -1)
          vg.daxacthuc = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt].ToString()).FirstOrDefault().Replace("&nbsp;", "");
        /*khoan tien*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "icon icon-dollar103");
        if (vt != -1)
          vg.mucluongtuden = arrGetHtmlDocumentALL[vt + 2].ToString().Trim();
        /*duyet boi*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Duyệt bởi", "<strong>", true);
        if (vt != -1)
          vg.duyetboi = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt].ToString()).FirstOrDefault();
        /*liên hệ*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "contact-name");
        if (vt != -1)
          vg.lienhe = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt].ToString()).FirstOrDefault();
        /*dia chỉ*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<span>Địa chỉ:</span>");
        if (vt != -1)
          vg.diachi = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 4].ToString()).FirstOrDefault();
        /*ct-body overflow clearfix*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "ct-body overflow clearfix");
        if (vt != -1) {
          vg.noidung = Regex.Replace(arrGetHtmlDocumentALL[vt + 2].Trim(), "<.*?>", " "); /*xoá ký tự html*/

          List<string> emails = Utilities_scanner.getEmail(new List<string> { vg.noidung });
          if (emails.Count() > 0) vg.email_nd = emails.FirstOrDefault().Trim();
          List<string> dienthoais = Utilities_scanner.getPhoneHTML(new List<string> { vg.noidung }, _dau_so, _regexs);
          if (dienthoais.Count() > 0) vg.dienthoai_nd = dienthoais.FirstOrDefault().Trim();
        }
        /*dien thoai*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "visible:ShowMobile()==false");
        if (vt != -1)
          vg.dienthoai = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt].ToString()).FirstOrDefault();
        /*thong tin ứng vien*/
        /*+ hoten*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Họ và Tên:", "col-md-5 col-sm-5 col-xs-5 item-name", true);
        if (vt != -1)
          vg.hoten = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 2].ToString()).FirstOrDefault();
        /*+ Năm sinh:*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Năm sinh:", "col-md-5 col-sm-5 col-xs-5 item-name", true);
        if (vt != -1)
          vg.namsinh = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 2].ToString()).FirstOrDefault();
        /*+Giới tính:*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Giới tính:", "col-md-5 col-sm-5 col-xs-5 item-name", true);
        if (vt != -1)
          vg.gioitinh = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 2].ToString()).FirstOrDefault();
        /*+Bằng cấp:*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Bằng cấp:", "col-md-5 col-sm-5 col-xs-5 item-name", true);
        if (vt != -1)
          vg.bangcap = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 2].ToString()).FirstOrDefault();
        /*+Ngoại ngữ:*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Ngoại ngữ:", "col-md-5 col-sm-5 col-xs-5 item-name", true);
        if (vt != -1)
          vg.bangcap = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 2].ToString()).FirstOrDefault();
        /*Loại hình công việc:*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Loại hình công việc:", "col-md-5 col-sm-5 col-xs-5 item-name", true);
        if (vt != -1)
          vg.loaihinhcv = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 2].ToString()).FirstOrDefault();
        /*Kinh nghiệm:*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Kinh nghiệm:", "col-md-5 col-sm-5 col-xs-5 item-name", true);
        if (vt != -1)
          vg.kinhnghiem = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 2].ToString()).FirstOrDefault();
        /*Vị trí:*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Vị trí:", "col-md-5 col-sm-5 col-xs-5 item-name", true);
        if (vt != -1)
          vg.vitri = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 2].ToString()).FirstOrDefault();

        ShowMessage(lbl_muaban_message2, vg.danhmucnghe, strPath, _thanhcong, _thatbai, _thanhcong + _thatbai);

        //return vg.danhmucnghe;
      }
      catch (Exception e) {
        _thatbai++;
        vg = null;
        writer = LogWriter.Instance;
        writer.WriteToLog(string.Format("{0}-{1}-{2}", e.Message, strPath, "getTrangCon-trangvang"));
        //return "";
      }
    }

  }
  public static class Utilities_batdongsan {
    public static bool hasProcess = true; /*khai bao bien stop*/

    public static int _PathLimit;/*so trang quet gioi han*/
    public static int _lanquetlai;
    public static int _Sleep;
    public static int _Timeout;


    public static int _IdDanhmuc;
    public static string _danhmuc;
    public static int _thanhcong = 0;
    public static int _thatbai = 0;
    public static Dictionary<string, int> _dau_so;
    public static List<regexs> _regexs;
    public static string _domain = "http://batdongsan.com.vn";
    public static infoPathmuaban _infoPathmuaban;

    private static LogWriter writer;


    public static DataTable Formatdm_batdongsan(DataTable tb_goc) {
      try {
        if (tb_goc.Rows.Count == 0)
          return tb_goc;
        DataTable tb_new = tb_goc.Clone();
        tb_new.Clear();
        DataRow[] _tb_cap1 = tb_goc.Select("parentId is null");
        foreach (DataRow item in _tb_cap1) {
          tb_new.ImportRow(item);
          DataRow[] _tb_cap2 = tb_goc.Select(string.Format("parentId='{0}'", item["Id"].ToString()));
          foreach (DataRow item2 in _tb_cap2) {
            tb_new.ImportRow(item2);
            string xx = string.Format("[spFindDmbds] '{0}'", item2["id"].ToString());
            DataTable tb_con = SQLDatabase.ExcDataTable(xx);
            foreach (DataRow item3 in tb_con.Rows)
              tb_new.ImportRow(item3);
          }
        }
        return tb_new;
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message, "Formatdm_batdongsan");
        return null;
      }
    }

    public static infoPathmuaban getPageMaxBanCho(string url, object arrControl) {
      string strHtml = "";
      string[] arrGetHtmlDocumentALL;

      ArrayList arr1 = (ArrayList)arrControl;
      Label lbl_bds_message1 = (Label)arr1[0];
      Label lbl_bds_message2 = (Label)arr1[1];
      Label lbl_bds_khoa = (Label)arr1[2];
      Label lbl_bds_Par = (Label)arr1[3];
      ProgressBar pro_muaban_tv = (ProgressBar)arr1[4];

      infoPathmuaban model = new infoPathmuaban();
      try {
        int solanlap = 0;
        strHtml = Utilities_batdongsan.WebRequestNavigate(url, ref solanlap, lbl_bds_khoa);
        arrGetHtmlDocumentALL = strHtml.Split('\n');

        int vt = Helpers.vitritim(arrGetHtmlDocumentALL.ToList(), "LeftMainContent__productSearchResult_blFilterAll");
        model.TotalResult = ConvertType.ToInt(Regex.Replace(arrGetHtmlDocumentALL[vt + 1].Trim(), "<.*?>", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty)); /*xoá ký tự html*/

        string strd1 = "class=\"background-pager-right-controls\"";
        string strd2 = "class=\"body-right\"";
        int vt11 = strHtml.IndexOf(strd1);
        int vt12 = strHtml.IndexOf(strd2, vt11);

        string strnoititel = strHtml.Substring(vt11, vt12 - vt11);
        List<string> arrGetHtmlDocumentALL2 = strnoititel.Split(new char[] { '\n', '\r' }).Where(r => r.Contains("<div")).ToList();
        int max = 0;
        foreach (var item in arrGetHtmlDocumentALL2) {
          Regex regex = new Regex(@"([p])[1-9]+");
          Match match = regex.Match(item);
          if (max < ConvertType.ToInt(match.Value.Replace("p", string.Empty)))
            max = ConvertType.ToInt(match.Value.Replace("p", string.Empty));
        }
        model.TotalPagingMax = max;

        return model;

      }
      catch (Exception ex) {
        return model;
      }
    }

    public static infoPathmuaban getPageMaxMuaThue(string url, object arrControl) {
      string strHtml = "";
      string[] arrGetHtmlDocumentALL;

      ArrayList arr1 = (ArrayList)arrControl;
      Label lbl_bds_message1 = (Label)arr1[0];
      Label lbl_bds_message2 = (Label)arr1[1];
      Label lbl_bds_khoa = (Label)arr1[2];
      Label lbl_bds_Par = (Label)arr1[3];
      ProgressBar pro_muaban_tv = (ProgressBar)arr1[4];

      infoPathmuaban model = new infoPathmuaban();
      try {
        int solanlap = 0;
        strHtml = Utilities_batdongsan.WebRequestNavigate(url, ref solanlap, lbl_bds_khoa);
        if (strHtml == "") return model;

        arrGetHtmlDocumentALL = Regex.Split(strHtml, "\r\n").Where(p => p.Contains("Tìm kiếm theo các tiêu chí:")).ToArray();
        string xx = Regex.Split(arrGetHtmlDocumentALL[0], " <span class='greencolor'><strong>").LastOrDefault();
        model.TotalResult = ConvertType.ToInt(Regex.Match(xx, @"\d+").Value);

        string strd1 = "class=\"background-pager-right-controls\"";
        string strd2 = "class=\"body-right\"";
        int vt11 = strHtml.IndexOf(strd1);
        int vt12 = strHtml.IndexOf(strd2, vt11);

        string strnoititel = strHtml.Substring(vt11, vt12 - vt11);
        List<string> arrGetHtmlDocumentALL2 = strnoititel.Split(new char[] { '\n', '\r' }).Where(r => r.Contains("<div")).ToList();
        int max = 0;
        foreach (var item in arrGetHtmlDocumentALL2) {
          Regex regex = new Regex(@"([p])[1-9]+");
          Match match = regex.Match(item);
          if (max < ConvertType.ToInt(match.Value.Replace("p", string.Empty)))
            max = ConvertType.ToInt(match.Value.Replace("p", string.Empty));
        }
        model.TotalPagingMax = max;

        return model;

      }
      catch (Exception ex) {
        return model;
      }
    }

    public static infoPathmuaban getPageMaxNhamoigioi(string url, object arrControl) {
      string strHtml = "";
      string[] arrGetHtmlDocumentALL;
      ArrayList arr1 = (ArrayList)arrControl;
      Label lbl_bds_message1 = (Label)arr1[0];
      Label lbl_bds_message2 = (Label)arr1[1];
      Label lbl_bds_khoa = (Label)arr1[2];
      Label lbl_bds_Par = (Label)arr1[3];
      ProgressBar pro_muaban_tv = (ProgressBar)arr1[4];

      infoPathmuaban model = new infoPathmuaban();
      try {
        int solanlap = 0;
        strHtml = Utilities_batdongsan.WebRequestNavigate(url, ref solanlap, lbl_bds_khoa);
        if (strHtml == "") return model;

        arrGetHtmlDocumentALL = Regex.Split(strHtml, "\r\n").Where(p => p.Contains("Tìm kiếm theo các tiêu chí:")).ToArray();
        string strd1 = "class=\"pager-block\"";
        string strd2 = "id=\"LeftMainContent__brokers_plhInform\"";
        int vt11 = strHtml.IndexOf(strd1);
        int vt12 = strHtml.IndexOf(strd2, vt11);

        string strnoititel = strHtml.Substring(vt11, vt12 - vt11);
        List<string> arrGetHtmlDocumentALL2 = Regex.Split(strnoititel, "<a href=").ToList(); //strnoititel.Split(new char[] { '\n', '\r' }).Where(r => r.Contains("<div")).ToList();
        int max = 0;
        foreach (var item in arrGetHtmlDocumentALL2) {
          Regex regex = new Regex(@"([p])[1-9]+");
          Match match = regex.Match(item);
          if (max < ConvertType.ToInt(match.Value.Replace("p", string.Empty)))
            max = ConvertType.ToInt(match.Value.Replace("p", string.Empty));
        }
        model.TotalPagingMax = max;

        return model;

      }
      catch (Exception ex) {
        //MessageBox.Show(ex.Message, "getPageNext");
        return model;
      }
    }

    public static string WebRequestNavigate(string url, ref int solanlap, Label lbl_khoa) {
      Stream stream = null;
      StringBuilder output = new StringBuilder();
      HttpWebRequest req;

      HttpWebResponse resp;
      lbl_khoa.Visible = false;

      while (stream == null && hasProcess) {
        try {
          StreamReader reader;
          req = (HttpWebRequest)WebRequest.Create(url);
          req.Credentials = CredentialCache.DefaultCredentials;
          req.AllowWriteStreamBuffering = false;
          req.Proxy = null;

          req.Timeout = _Timeout;
          req.ReadWriteTimeout = _Timeout;
          req.ContentType = "text/html";
          req.Method = WebRequestMethods.Http.Get;


          /*fix loi: The remote server returned an error: (403) Forbidden.*/
          req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:54.0) Gecko/20100101 Firefox/54.0";
          req.Accept = "*/*";
          req.AllowWriteStreamBuffering = true;
          req.ProtocolVersion = HttpVersion.Version11;
          req.AllowAutoRedirect = true;
          req.ContentType = "application/x-www-form-urlencoded";
          req.Timeout = Timeout.Infinite;
          req.KeepAlive = true;

          ServicePointManager.Expect100Continue = true;
          ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
          ServicePointManager.DefaultConnectionLimit = 200;

          resp = (HttpWebResponse)req.GetResponse();
          stream = resp.GetResponseStream();

          using (reader = new StreamReader(resp.GetResponseStream())) {
            output.Clear();
            output.Append(reader.ReadToEnd());
          }
          return HttpUtility.HtmlDecode(output.ToString());
        }
        catch (WebException ex) {
          if (ex.ToString().Contains("time")) {
            if (solanlap <= _lanquetlai && _lanquetlai != -1) {
              solanlap = solanlap + 1;

              lbl_khoa.Visible = true;
              lbl_khoa.Text = string.Format("Đứt Kết Nối : + {0}", solanlap - 1);
              lbl_khoa.Update();

              Thread.Sleep(_Sleep);
              WebRequestNavigate(url, ref solanlap, lbl_khoa);
            }
            else if (_lanquetlai == -1) {
              lbl_khoa.Visible = true;
              lbl_khoa.Text = string.Format("Đứt Kết Nối..");
              lbl_khoa.Update();
              solanlap = 0;
              Thread.Sleep(_Sleep);
              WebRequestNavigate(url, ref solanlap, lbl_khoa);
            }
            else
              _thatbai++;
          }
        }
        catch (Exception e) {
          return "";
        }
      }
      return "";
    }

    public static void getwebBrowser_Canban(string strPath, ref int solanlap, object arrControl) {
      ArrayList arr1 = (ArrayList)arrControl;
      Label lbl_bds_message1 = (Label)arr1[0];
      Label lbl_bds_message2 = (Label)arr1[1];
      Label lbl_bds_khoa = (Label)arr1[2];
      Label lbl_bds_Par = (Label)arr1[3];
      ProgressBar pro_muaban_tv = (ProgressBar)arr1[4];
      Dictionary<string, string> dictionary = new Dictionary<string, string>();

      try {

        if (!Utilities_batdongsan.hasProcess) return;/*stop*/

        string strHTML = WebRequestNavigate(strPath, ref solanlap, lbl_bds_khoa);
        if (strHTML == "" || strHTML == null) return;

        string chuoimoi1 = "<div class=\"Main\">";
        int vt1 = strHTML.IndexOf(chuoimoi1);
        string str1 = strHTML.Substring(vt1, strHTML.Length - vt1);
        int vt2 = str1.IndexOf("background-pager-controls");

        string str2 = str1.Substring(0, vt2 + chuoimoi1.Length);
        List<string> arrGetDocument = str2.Split(new char[] { '\n', '\r' }).Where(p => p.Contains("<h3><a href=")).ToList();

        batdongsan model;
        foreach (var item in arrGetDocument) {
          string strUrl = _domain + Helpers.getUrlHtml3(item).FirstOrDefault();   /*xu ly linh chi tiet loi sai link \t*/
          model = new batdongsan();
          model.danhmucid = _IdDanhmuc;
          model.sys_diachiweb = strUrl;
          solanlap = 0;
          Utilities_batdongsan.getTrangCon_Canban(strUrl, ref model, ref solanlap, arrControl);
          if (Utilities_batdongsan.hasProcess)
            if (SQLDatabase.AddBatdongsan(model))
              _thanhcong++;
            else
              _thatbai++;
        }
      }
      catch (Exception e) {
        return;
      }
    }

    public static void getTrangCon_Canban(string strPath, ref batdongsan model, ref int solanlap, object arrControl) {
      ArrayList arr1 = (ArrayList)arrControl;
      Label lbl_bds_message1 = (Label)arr1[0];
      Label lbl_bds_message2 = (Label)arr1[1];
      Label lbl_bds_khoa = (Label)arr1[2];
      Label lbl_bds_Par = (Label)arr1[3];
      ProgressBar pro_muaban_tv = (ProgressBar)arr1[4];

      string strWebsite;

      try {
        //strPath = "http://batdongsan.com.vn/ban-can-ho-chung-cu-duong-dai-tu-phuong-dai-kim-prj-eco-lake-view/mo-toa-hh3-gia-chi-1-3-ty-gia-goc-cdt-tro-l-s-0-chiet-khau-1-5-lh-090-55-24-666-pr12788027";
        if (!Utilities_batdongsan.hasProcess) {
          model = null;
          return;/*stop*/
        }
        strWebsite = WebRequestNavigate(strPath, ref solanlap, lbl_bds_khoa);
        /**************************Lấy danh mục nghề nghiệp***********************************/
        string strd1 = "id=\"product-detail\"";
        string strd2 = "id=\"hdLat\"";
        int vt11 = strWebsite.IndexOf(strd1);
        int vt12 = strWebsite.IndexOf(strd2, vt11);

        string strnoititel = strWebsite.Substring(vt11, vt12 - vt11);
        List<string> arrGetHtmlDocumentALL2 = strnoititel.Split(new char[] { '\n', '\r' }).ToList();
        int vt = 0;
        /*class=\"pm-title\"*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "class=\"pm-title\"");
        if (vt != -1) {
          model.tieude = arrGetHtmlDocumentALL2[vt + 4];
        }
        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "<b>Khu vực:</b>");
        if (vt != -1) {
          List<string> arrKhuvuc1 = Helpers.getDataHTML(arrGetHtmlDocumentALL2[vt].ToString()).ToList();
          if (arrKhuvuc1.Count == 4) {
            string[] arrkhuvuc2 = arrKhuvuc1[3].Split('-').Where(p => p.Length > 0).ToArray();
            if (arrkhuvuc2.Count() == 1) {
              model.khuvuc_canban = arrkhuvuc2[0];
              model.thanhpho_canban = arrkhuvuc2[0];
            }
            else {
              model.quan_canban = arrkhuvuc2[0];
              model.thanhpho_canban = arrkhuvuc2[1];
              model.khuvuc_canban = string.Format("{0}-{1}", arrkhuvuc2[0], arrkhuvuc2[1]);
            }
          }
        }
        /*----giá----*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "Giá:</b>");
        if (vt != -1) {
          model.giaban = arrGetHtmlDocumentALL2[vt + 4].Replace("&nbsp;", string.Empty);
        }
        /*Diện tích:</b>*/

        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "Diện tích:</b>");
        if (vt != -1) {
          model.dientich = arrGetHtmlDocumentALL2[vt + 4].Replace("</strong>", string.Empty);
        }
        /**************************************************************/
        /*mô tả*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "div class=\"pm-desc\"");
        if (vt != -1) {
          model.noidung = Regex.Replace(arrGetHtmlDocumentALL2[vt + 2].Trim(), "<.*?>", " "); /*xoá ký tự html*/

          List<string> emails = Utilities_scanner.getEmail(new List<string> { model.noidung });
          if (emails.Count() != 0) model.email_nd = emails.FirstOrDefault().Trim();

          List<string> phones = Utilities_scanner.getPhoneHTML(new List<string> { model.noidung }, _dau_so, _regexs);
          if (phones.Count() != 0) model.dienthoai_nd = phones.FirstOrDefault().Trim();
        }
        /*Mã tin đăng:*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "Mã tin đăng:", "<div ondblclick=\"try{document.execCommand('copy');}catch(ex){}\">", false);
        if (vt != -1) {
          model.matindang = Regex.Match(arrGetHtmlDocumentALL2[vt + 4].Trim(), @"(\d+)").Value;
        }
        /*loại tin đăng*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "<div style=\"width: 220px\"><span class=\"normalblue\">Loại hình tin đăng:</span>");
        if (vt != -1) {
          model.loaihinhtindang = Helpers.getDataHTML(arrGetHtmlDocumentALL2[vt].Trim()).LastOrDefault();
        }
        /*************ngày đăng********************/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "<div style=\"width: 175px\"><span class=\"normalblue\">Ngày đăng:</span>");
        if (vt != -1) {
          model.ngaydang = Helpers.getDataHTML(arrGetHtmlDocumentALL2[vt].Trim()).LastOrDefault();
        }
        /*************Ngày hết hạn:****************/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "<div><span class=\"normalblue\">Ngày hết hạn:</span>");
        if (vt != -1) {
          model.ngayhethan = Helpers.getDataHTML(arrGetHtmlDocumentALL2[vt].Trim()).LastOrDefault();
        }

        /**********************Group Liên hệ*********************************/
        /*1-Tên liên lạc*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "LeftMainContent__productDetail_contactName");
        if (vt != -1) {
          model.lienhe_tenlienlac = arrGetHtmlDocumentALL2[vt + 10].Trim();
        }
        /*2-Điện thoại*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "LeftMainContent__productDetail_contactPhone");
        if (vt != -1) {
          model.lienhe_dienthoai = Utilities_scanner.getPhoneHTML(new List<string> { arrGetHtmlDocumentALL2[vt + 10] }, _dau_so, _regexs).FirstOrDefault();
        }
        /*3-Mobile*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "LeftMainContent__productDetail_contactMobile");
        if (vt != -1) {
          model.lienhe_mobilde = Utilities_scanner.getPhoneHTML(new List<string> { arrGetHtmlDocumentALL2[vt + 10] }, _dau_so, _regexs).FirstOrDefault();
        }
        /*4-Email*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "id=\"contactEmail\"");
        if (vt != -1) {
          model.lienhe_email = Helpers.getDataHTML(arrGetHtmlDocumentALL2[vt + 12].Trim()).FirstOrDefault();
        }
        /*lien he*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "id=\"LeftMainContent__productDetail_contactAddress\"");
        if (vt != -1) {
          model.lienhe_diachi = arrGetHtmlDocumentALL2[vt + 10].Trim();
        }

        /****************Grouo đặc điểm bất động sản******************/

        string grdacdiem1 = "class=\"div-table-cell table1\"";
        string grdacdiem2 = "class=\"div-table-cell\"";
        int vt111 = strWebsite.IndexOf(grdacdiem1);
        int vt122 = strWebsite.IndexOf(grdacdiem2, vt111);

        string strddbds = strWebsite.Substring(vt111, vt122 - vt111);
        List<string> arrGetHtmlDocumentALL22 = strddbds.Split(new char[] { '\n', '\r' }).ToList();

        /**********************Thông tin dự án*******************************/
        /*-Tên dự án*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL22, "Tên dự án");
        if (vt != -1) {
          model.ttduan_tenduan = arrGetHtmlDocumentALL22[vt + 4].Trim();
        }
        /*-Chủ đầu tư*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL22, "Chủ đầu tư");
        if (vt != -1) {
          model.ttduan_chudautu = arrGetHtmlDocumentALL22[vt + 4].Trim();
        }
        /*Quy mô*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL22, "Quy mô");
        if (vt != -1) {
          model.ttduan_quymo = arrGetHtmlDocumentALL22[vt + 4].Trim();
        }
        /*-----------------------------------------------------*/

        /*1-Loại tin rao*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL22, "Loại tin rao");
        if (vt != -1) {
          model.dd_bds_loaitinrao = arrGetHtmlDocumentALL22[vt + 6].Trim();
        }
        /*2-Địa chỉ*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL22, "Địa chỉ");
        if (vt != -1) {
          model.dd_bds_diachi = arrGetHtmlDocumentALL22[vt + 6].Trim();
        }
        /*Mặt tiền*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL22, "Mặt tiền");
        if (vt != -1) {
          model.dd_bds_mattien = arrGetHtmlDocumentALL22[vt + 6].Trim();
        }
        /*Đường vào*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL22, "Đường vào");
        if (vt != -1) {
          model.dd_bds_duongvao = arrGetHtmlDocumentALL22[vt + 6].Trim();
        }
        /*Hướng nhà */
        vt = Helpers.vitritim(arrGetHtmlDocumentALL22, "Hướng nhà");
        if (vt != -1) {
          model.dd_bds_huongnha = arrGetHtmlDocumentALL22[vt + 6].Trim();
        }
        /*Hướng ban công */
        vt = Helpers.vitritim(arrGetHtmlDocumentALL22, "Hướng ban công");
        if (vt != -1) {
          model.dd_bds_huongbancong = arrGetHtmlDocumentALL22[vt + 6].Trim();
        }
        /*Số tầng*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL22, "Số tầng");
        if (vt != -1) {
          model.dd_bds_sotang = arrGetHtmlDocumentALL22[vt + 6].Trim();
        }
        /*3-Số phòng ngủ*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL22, "Số phòng ngủ");
        if (vt != -1) {
          model.dd_bds_sophongngu = arrGetHtmlDocumentALL22[vt + 6].Trim();
        }
        /*4-Số toilet*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL22, "Số toilet");
        if (vt != -1) {
          model.dd_bds_sotoilet = arrGetHtmlDocumentALL22[vt + 6].Trim();
        }
        /*5-Nội thất*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL22, "Nội thất");
        if (vt != -1) {
          model.dd_bds_noithat = arrGetHtmlDocumentALL22[vt + 6].Trim();
        }

        ShowMessage(lbl_bds_message2, _danhmuc, strPath, _thanhcong, _thatbai, _thanhcong + _thatbai);
      }
      catch (Exception e) {
        _thatbai++;
        writer = LogWriter.Instance;
        writer.WriteToLog(string.Format("{0}-{1}-{2}", e.Message, strPath, "getTrangCon-trangvang"));
      }
    }

    public static string getGiaiMa(string values) {
      try {

        string[] words = Regex.Split(values, ";&#");
        byte[] arrByte = new byte[words.Count()];
        for (int i = 0; i < words.Count(); i++) {
          arrByte[i] = Convert.ToByte(words[i].Replace("&#", "").Replace(";", ""));
        }
        Encoding latin1 = Encoding.GetEncoding(28591);
        return latin1.GetString(arrByte);
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message, "getGiaiMa");
        return "";
      }

    }

    public static void getwebBrowserNhamoigioi(string strPath, ref int solanlap, object arrControl) {

      ArrayList arr1 = (ArrayList)arrControl;
      Label lbl_bds_message1 = (Label)arr1[0];
      Label lbl_bds_message2 = (Label)arr1[1];
      Label lbl_bds_khoa = (Label)arr1[2];
      Label lbl_bds_Par = (Label)arr1[3];
      ProgressBar pro_muaban_tv = (ProgressBar)arr1[4];

      string output = "";
      Dictionary<string, string> dictionary = new Dictionary<string, string>();

      try {

        if (!Utilities_batdongsan.hasProcess)
          return;/*stop*/
        output = WebRequestNavigate(strPath, ref solanlap, lbl_bds_khoa);
        if (output == null || output.ToString() == "") return;

        string chuoimoi1 = "<div class=\"container-default\">";
        int vt1 = output.ToString().IndexOf(chuoimoi1);
        string str1 = output.ToString().Substring(vt1, output.ToString().Length - vt1);
        int vt2 = str1.IndexOf("<div class=\"body-right\">");
        string str2 = str1.Substring(0, vt2 + chuoimoi1.Length);
        List<string> arrGetDocument = Regex.Split(str2, "<div class=\"borderpad10 mar-bot\">").Where(p => p.Contains("LeftMainContent__brokers_repBrokes")).ToList();
        batdongsan model;

        foreach (var item in arrGetDocument) {
          string strUrl = _domain + Helpers.getUrlHtml2(item).FirstOrDefault();   /*xu ly linh chi tiet loi sai link \t*/
          model = new batdongsan();
          model.danhmucid = _IdDanhmuc;
          model.sys_diachiweb = strUrl;
          solanlap = 0;
          List<string> arrGetHtmlDocumentALL = item.Split(new char[] { '\n', '\r' }).ToList();

          int vt = 0;
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "title=");
          if (vt != -1) {
            model.moigioi_tieude = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt].Trim()).FirstOrDefault();
          }

          /*list-title-detail*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Địa chỉ:");
          if (vt != -1) {
            model.moigioi_diachi = Regex.Replace(arrGetHtmlDocumentALL[vt + 6].Trim(), @"<[^>]*>", String.Empty);
          }
          /*số bàn*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Số bàn:");
          if (vt != -1) {

            model.moigioi_soban = arrGetHtmlDocumentALL[vt + 4].Trim();
            List<string> arrsoban = Utilities_scanner.getPhoneHTML(new List<string> { arrGetHtmlDocumentALL[vt + 4].Trim() }, _dau_so, _regexs);
            if (arrsoban.Count() > 0)
              model.moigioi_soban_bydidong = arrsoban.FirstOrDefault();
          }
          /*Di động:*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Di động:");
          if (vt != -1) {
            model.moigioi_didong = arrGetHtmlDocumentALL[vt + 4].Trim();
            List<string> arrdidong = Utilities_scanner.getPhoneHTML(new List<string> { arrGetHtmlDocumentALL[vt + 4].Trim() }, _dau_so, _regexs);
            if (arrdidong.Count() > 0)
              model.moigioi_didong_bydidong = arrGetHtmlDocumentALL[vt + 4].Trim();
          }
          /*Email*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Email");
          if (vt != -1) {
            model.moigioi_email = Utilities_scanner.getEmail(new List<string>() { arrGetHtmlDocumentALL[vt + 14].Trim() }).FirstOrDefault();
          }
          /*Website:*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Website:");
          if (vt != -1) {
            model.moigioi_website = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt].Trim()).LastOrDefault();
          }

          Utilities_batdongsan.getTrangConNhamoigioi(strUrl, ref model, ref solanlap, arrControl);
          if (Utilities_batdongsan.hasProcess)
            if (model != null && SQLDatabase.AddBatdongsan(model))   /*lưu thông tin vào csdl*/
              _thanhcong++;
            else
              _thatbai++;
        }
      }
      catch (Exception e) {
        return;
      }
    }

    public static void getTrangConNhamoigioi(string strPath, ref batdongsan model, ref int solanlap, object arrControl) {
      ArrayList arr1 = (ArrayList)arrControl;
      Label lbl_bds_message1 = (Label)arr1[0];
      Label lbl_bds_message2 = (Label)arr1[1];
      Label lbl_bds_khoa = (Label)arr1[2];
      Label lbl_bds_Par = (Label)arr1[3];
      ProgressBar pro_muaban_tv = (ProgressBar)arr1[4];

      string strWebsite;

      try {
        //strPath = "http://batdongsan.com.vn/ban-can-ho-chung-cu-cau-giay/tran-thuy-ib230854";
        if (!Utilities_batdongsan.hasProcess)
          return;/*stop*/
        strWebsite = WebRequestNavigate(strPath, ref solanlap, lbl_bds_khoa);
        if (strWebsite == "" || strWebsite == null) {
          model = null;
          return;
        }
        /**************************Lấy danh mục nghề nghiệp***********************************/
        string strd1 = "class=\"broker-detail\"";
        string strd2 = "class=\"border2px\"";
        int vt11 = strWebsite.IndexOf(strd1);
        int vt12 = strWebsite.IndexOf(strd2, vt11);

        string strnoititel = strWebsite.Substring(vt11, vt12 - vt11);
        List<string> arrGetHtmlDocumentALL2 = strnoititel.Split(new char[] { '\n', '\r' }).ToList();
        int vt = 0;
        /*Fax*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "Fax</div>");
        if (vt != -1) {
          model.moigioi_fax = arrGetHtmlDocumentALL2[vt + 4].Replace("</div>", string.Empty);
        }
        /*Mã số thuế*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "Mã số thuế");
        if (vt != -1) {
          model.moigioi_masothue = arrGetHtmlDocumentALL2[vt + 4].Replace("</div>", string.Empty);
        }
        /****************Grouo đặc điểm bất động sản******************/

        string grdacdiem1 = "class=\"ltrAreaIntro\"";
        string grdacdiem2 = "class=\"introcontent\"";
        int vt111 = strWebsite.IndexOf(grdacdiem1);
        int vt122 = strWebsite.IndexOf(grdacdiem2, vt111);

        string strddbds = strWebsite.Substring(vt111, vt122 - vt111);
        List<string> arrGetHtmlDocumentALL22 = Helpers.getDataHTML(strddbds.Split(new char[] { '\n', '\r' }).Where(p => p.Contains("<ul><li><span>")).FirstOrDefault());

        /**********************Thông tin dự án*******************************/
        foreach (var item in arrGetHtmlDocumentALL22) {
          model.moigioi_khuvucmoigioi += item + ";";
        }

        ShowMessage(lbl_bds_message2, _danhmuc, strPath, _thanhcong, _thatbai, _thanhcong + _thatbai);

      }
      catch (Exception e) {
        _thatbai++;
        model = null;
        writer = LogWriter.Instance;
        writer.WriteToLog(string.Format("{0}-{1}-{2}", e.Message, strPath, "getTrangCon-moigioi"));
      }
    }


    public static void getwebBrowser_Canmua(string strPath, ref int solanlap, object arrControl) {
      ArrayList arr1 = (ArrayList)arrControl;
      Label lbl_bds_message1 = (Label)arr1[0];
      Label lbl_bds_message2 = (Label)arr1[1];
      Label lbl_bds_khoa = (Label)arr1[2];
      Label lbl_bds_Par = (Label)arr1[3];
      ProgressBar pro_muaban_tv = (ProgressBar)arr1[4];

      string output;

      Dictionary<string, string> dictionary = new Dictionary<string, string>();

      try {

        if (!Utilities_batdongsan.hasProcess)
          return;/*stop*/

        output = WebRequestNavigate(strPath, ref solanlap, lbl_bds_khoa);
        if (output == "" || output == null) return;

        string chuoimoi1 = "<div class=\"Main\">";
        int vt1 = output.ToString().IndexOf(chuoimoi1);
        string str1 = output.ToString().Substring(vt1, output.ToString().Length - vt1);
        int vt2 = str1.IndexOf("background-pager-controls");

        string str2 = str1.Substring(0, vt2 + chuoimoi1.Length);

        List<string> arrGetDocumentCon = str2.Split(new char[] { '\n', '\r' }).Where(p => p.Contains("a href='")).ToList();

        batdongsan model;
        foreach (var item in arrGetDocumentCon) {
          model = new batdongsan();
          string strUrl = _domain + Helpers.getUrlHtml3(item).FirstOrDefault();   /*xu ly linh chi tiet loi sai link \t*/
                                                                                  /*--------------------*/
          int vt11 = str2.IndexOf(item);
          int vt22 = str2.IndexOf("p-title", vt11) == -1 ? str2.Length : str2.IndexOf("p-title", vt11);
          string strcon = str1.Substring(vt11, vt22);
          List<string> arrGetDocumentCon2 = strcon.Split(new char[] { '\n', '\r' }).ToList();
          /*những khu vực cần mùa*/
          int vtcon = Helpers.vitritim(arrGetDocumentCon2, "Quận/Huyện:");
          if (vtcon != -1) {
            model.quanhuyen_canmua = arrGetDocumentCon2[vtcon + 4];
          }
          /*những thành phố cần mua*/
          vtcon = Helpers.vitritim(arrGetDocumentCon2, "Tỉnh/TP:");
          if (vtcon != -1) {
            model.thanhpho_canmua = arrGetDocumentCon2[vtcon + 4];
          }
          model.danhmucid = _IdDanhmuc;
          model.sys_diachiweb = strUrl;
          solanlap = 0;
          Utilities_batdongsan.getTrangCon_Canmua(strUrl, ref model, ref solanlap, arrControl);
          if (Utilities_batdongsan.hasProcess)
            if (model != null && SQLDatabase.AddBatdongsan(model))   /*lưu thông tin vào csdl*/
              _thanhcong++;
            else
              _thatbai++;

        }
      }
      catch (Exception e) {
        return;
      }
    }
    public static void getTrangCon_Canmua(string strPath, ref batdongsan model, ref int solanlap, object arrControl) {
      ArrayList arr1 = (ArrayList)arrControl;
      Label lbl_bds_message1 = (Label)arr1[0];
      Label lbl_bds_message2 = (Label)arr1[1];
      Label lbl_bds_khoa = (Label)arr1[2];
      Label lbl_bds_Par = (Label)arr1[3];
      ProgressBar pro_muaban_tv = (ProgressBar)arr1[4];

      string strWebsite;

      try {
        //strPath = "http://batdongsan.com.vn/mua-dat-buon-ma-thuot-ddl/can-mua-dat-gan-khu-nguyen-huu-tho-buon-ma-thuot-dak-lak-ad53114";
        if (!Utilities_batdongsan.hasProcess)
          return;/*stop*/

        strWebsite = WebRequestNavigate(strPath, ref solanlap, lbl_bds_khoa);
        if (strWebsite == "" || strWebsite == null) {
          model = null;
          return;
        }
        /**************************Lấy danh mục nghề nghiệp***********************************/
        string strd1 = "id=\"product-detail\"";
        string strd2 = "class=\"body-right\"";
        int vt11 = strWebsite.IndexOf(strd1);
        int vt12 = strWebsite.IndexOf(strd2, vt11);

        string strnoititel = strWebsite.Substring(vt11, vt12 - vt11);
        List<string> arrGetHtmlDocumentALL2 = strnoititel.Split(new char[] { '\n', '\r' }).ToList();
        int vt = 0;
        /*class=\"pm-title\"*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "class=\"pm-title\"");
        if (vt != -1) {
          model.tieude = Regex.Replace(arrGetHtmlDocumentALL2[vt + 4], "<.*?>", string.Empty);
        }
        /*----giá----*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "Giá:</b>");
        if (vt != -1) {
          model.giaban = arrGetHtmlDocumentALL2[vt + 4].Replace("&nbsp;", string.Empty);
        }
        /*Diện tích:</b>*/

        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "Diện tích:</b>");
        if (vt != -1) {
          model.dientich = arrGetHtmlDocumentALL2[vt + 4].Replace("</strong>", string.Empty);
        }

        /*mô tả*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL2, "class=\"pm-content stat\"");
        if (vt != -1) {
          model.noidung = Regex.Replace(arrGetHtmlDocumentALL2[vt + 2].Trim(), "<.*?>", " "); /*xoá ký tự html*/

          List<string> emails = Utilities_scanner.getEmail(new List<string> { model.noidung });
          if (emails.Count() != 0) model.email_nd = emails.FirstOrDefault().Trim();

          List<string> phones = Utilities_scanner.getPhoneHTML(model.noidung, _dau_so, _regexs);
          if (phones.Count() != 0) model.dienthoai_nd = phones.FirstOrDefault().Trim();

        }
        /******************************Dat diem bat dong san***********************/
        string strvt1 = "class=\"pm-content-detail\"";
        string strvt2 = "<!--end content-detail-->";
        int vtdd1 = strWebsite.IndexOf(strvt1);
        int vtdd2 = strWebsite.IndexOf(strvt2, vtdd1);

        string strddtlienhe = strWebsite.Substring(vtdd1, vtdd2 - vtdd1);
        List<string> arrGetHtmlDocumentALLddlh = strddtlienhe.Split(new char[] { '\n', '\r' }).ToList();

        /*Mã số:*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALLddlh, "Mã số");
        if (vt != -1) {
          model.matindang = arrGetHtmlDocumentALLddlh[vt + 6].Trim();
        }
        /*loại tin đăng*/

        vt = Helpers.vitritim(arrGetHtmlDocumentALLddlh, "Loại tin rao");
        if (vt != -1) {
          model.loaihinhtindang = arrGetHtmlDocumentALLddlh[vt + 6].Trim();
        }
        /*************ngày đăng********************/
        vt = Helpers.vitritim(arrGetHtmlDocumentALLddlh, "Ngày đăng tin");
        if (vt != -1) {
          model.ngaydang = arrGetHtmlDocumentALLddlh[vt + 6].Trim();
        }
        /*************Ngày hết hạn:****************/
        vt = Helpers.vitritim(arrGetHtmlDocumentALLddlh, "Ngày hết hạn");
        if (vt != -1) {
          model.ngayhethan = arrGetHtmlDocumentALLddlh[vt + 6].Trim();
        }
        /*lienhe_tenlienhe*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALLddlh, "LeftMainContent__detail_contactName");
        if (vt != -1) {
          model.lienhe_tenlienlac = arrGetHtmlDocumentALLddlh[vt + 10].Trim();
        }
        /*LeftMainContent__detail_contactPhone*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALLddlh, "LeftMainContent__detail_contactPhone");
        if (vt != -1) {
          model.lienhe_dienthoai = arrGetHtmlDocumentALLddlh[vt + 10].Trim();
        }
        /*LeftMainContent__detail_contactMobile*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALLddlh, "LeftMainContent__detail_contactMobile");
        if (vt != -1) {
          model.lienhe_mobilde = arrGetHtmlDocumentALLddlh[vt + 10].Trim();
        }
        /*LeftMainContent__detail_contactEmail*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALLddlh, "LeftMainContent__detail_contactEmail");
        if (vt != -1) {
          model.lienhe_email = Helpers.getDataHTML(arrGetHtmlDocumentALLddlh[vt + 12].Trim()).FirstOrDefault();
        }
        ShowMessage(lbl_bds_message2, _danhmuc, strPath, _thanhcong, _thatbai, _thanhcong + _thatbai);


      }
      catch (Exception e) {
        _thatbai++;
        model = null;
        writer = LogWriter.Instance;
        writer.WriteToLog(string.Format("{0}-{1}-{2}", e.Message, strPath, "getTrangCon-canthue"));
      }
    }

    public static void ShowMessage(Label lbl_bds_message2, string trang, string chitiet, int thanhcong, int thatbai, int hientai) {
      lbl_bds_message2.Text = string.Format("Trang: {0} +Hiện tại: -->{1}<--[{2}] - Thành công: {3}/Thất bại:{4} \n {5}", trang, String.Format("{0:#,##0.##}", hientai), String.Format("{0:#,##0.##}", _infoPathmuaban.TotalResult), String.Format("{0:#,##0.##}", thanhcong), String.Format("{0:#,##0.##}", thatbai), chitiet);
      lbl_bds_message2.Update();
    }

  }
  public static class Utilities_vinabiz {
    //public static bool hasProcess = true; /*khai bao bien stop*/
    public static int _PathLimit;/*so trang quet gioi han*/
    public static int _lanquetlai;
    public static int _Sleep;
    public static int _Timeout;
    public static Dictionary<string, int> _dau_so;
    public static List<regexs> _regexs;
    public static bool ChkCKiemTraTrung = false;
    public static int IdDanhmuc;
    public static int _thanhcong = 0;
    public static int _thatbai = 0;
    public static List<string> ds_dauso;
    public static string _domain = "https://vinabiz.org/";
    public static List<dm_hsct> _listHsct;
    public static List<dm_vinabiz_map> _listquetcan;
    public static List<dm_Tinh> _listTinh;

    private static LogWriter writer;

    public static string WebRequestNavigate(string url, ref int solanlap, Label lbl_khoa) {
      Stream stream = null;
      StringBuilder output = new StringBuilder();
      lbl_khoa.Visible = false;
      while (stream == null) {
        try {

          StreamReader reader;
          HttpWebResponse resp = null;
          HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
          resp = (HttpWebResponse)myReq.GetResponse();

          stream = resp.GetResponseStream();
          stream.ReadTimeout = _Timeout;
          reader = new StreamReader(resp.GetResponseStream());
          output.Clear();
          output.Append(reader.ReadToEnd());


        }
        catch (WebException ex) {
          if (ex.ToString().Contains("time")) {
            if (solanlap <= _lanquetlai && _lanquetlai != -1) {
              solanlap = solanlap + 1;

              lbl_khoa.Visible = true;
              lbl_khoa.Text = string.Format("Đứt Kết Nối : + {0}", solanlap - 1);
              lbl_khoa.Update();

              Thread.Sleep(_Sleep);
              WebRequestNavigate(url, ref solanlap, lbl_khoa);
            }
            else if (_lanquetlai == -1) {
              lbl_khoa.Visible = true;
              lbl_khoa.Text = string.Format("Đứt Kết Nối..");
              lbl_khoa.Update();
              solanlap = 0;
              Thread.Sleep(_Sleep);
              WebRequestNavigate(url, ref solanlap, lbl_khoa);
            }
            else
              _thatbai++;
          }
        }
        catch (Exception e) {
          return "";
        }
      }

      return System.Net.WebUtility.HtmlDecode(output.ToString());
    }

    public static string WebRequestNavigateNew(string url, ref int solanlap, Label lbl_khoa) {
      string html = null;
      lbl_khoa.Visible = false;
      while (html == null) {
        try {
          html = HttpUtility.HtmlDecode(Web.WebToolkit.GetHtml(@url));
        }
        catch (WebException ex) {
          if (ex.ToString().Contains("time")) {
            if (solanlap <= _lanquetlai && _lanquetlai != -1) {
              solanlap = solanlap + 1;

              lbl_khoa.Visible = true;
              lbl_khoa.Text = string.Format("Đứt Kết Nối : + {0}", solanlap - 1);
              lbl_khoa.Update();

              Thread.Sleep(_Sleep);
              WebRequestNavigateNew(url, ref solanlap, lbl_khoa);
            }
            else if (_lanquetlai == -1) {
              lbl_khoa.Visible = true;
              lbl_khoa.Text = string.Format("Đứt Kết Nối..");
              lbl_khoa.Update();
              solanlap = 0;
              Thread.Sleep(_Sleep);
              WebRequestNavigateNew(url, ref solanlap, lbl_khoa);
            }
            else
              _thatbai++;
          }
        }
        catch (Exception e) {
          return "";
        }
      }
      return html;
    }
    public static infoPathmuaban getPageMax(string url, object arrControl) {
      string strHtml = "";

      ArrayList arr1 = (ArrayList)arrControl;
      RichTextBox txtMessage = (RichTextBox)arr1[0];
      Label lbl_vinabiz_khoa = (Label)arr1[1];

      infoPathmuaban model = new infoPathmuaban();
      int max = 0;

      try {
        int solanlap = 0;
        strHtml = Utilities_vinabiz.WebRequestNavigateNew(url, ref solanlap, lbl_vinabiz_khoa).ToString();
        HtmlAgilityPack.HtmlDocument _doc = new HtmlAgilityPack.HtmlDocument();
        _doc.LoadHtml(strHtml);


        var nodelPage = _doc.DocumentNode.Descendants("ul").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("pagination")).LastOrDefault();
        if (nodelPage == null) return model;
        var nodelPagedList = nodelPage.SelectNodes(".//li").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("PagedList-skipToLast")).FirstOrDefault();

        if (nodelPagedList.SelectNodes(".//a").FirstOrDefault().Attributes["href"] == null) return model;
        var href = nodelPagedList.SelectNodes(".//a").FirstOrDefault().Attributes["href"].Value;

        string[] words = href.Split('/');

        int vt = ConvertType.ToInt(words.LastOrDefault());
        max = vt <= max ? max : vt;
        model.TotalPagingMax = max;

        return model;

      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message, "getPageNext");
        return model;
      }
    }


    public static void ShowMessage(Label lbl, string chitiet, int thanhcong, int thatbai) {
      lbl.Text = string.Format("Thành công: {0} /Thất bại: {1} \n -> {2}", thanhcong, thatbai, chitiet);
      lbl.Update();
    }
    public static void getwebBrowser(Gridview model, ref int solanlap, object arrControl) {
      ArrayList arr1 = (ArrayList)arrControl;
      RichTextBox txtMessage = (RichTextBox)arr1[0];
      Label lbl_vinabiz_khoa = (Label)arr1[1];
      Label lbl_thanhcong = (Label)arr1[2];
      Label lbl_thatbai = (Label)arr1[3];


      string output;
      Dictionary<string, string> dictionary = new Dictionary<string, string>();

      try {
        //strPath = "https://vinabiz.org/search/find?q=vdc";
        output = WebRequestNavigateNew(model.path, ref solanlap, lbl_vinabiz_khoa);
        /*xử lý code mới ở đây*/

        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(output);
        HtmlNode documentNode = doc.DocumentNode;

        string xPath = "//a[@data-placement='bottom']";
        HtmlNodeCollection listNode = documentNode.SelectNodes(xPath);
        vinabiz vna;
        foreach (HtmlNode item in listNode) {
          vna = new vinabiz();
          string linkct = _domain + item.Attributes["href"].Value;
          string tieude = item.Attributes["data-original-title"].Value;

          List<string> arr = tieude.Split(new char[] { '>' }).ToList().Where(p => p.Contains("Mã doanh nghiệp:")).ToList();
          if (arr.FirstOrDefault().Contains("-"))
            vna.ttdk_msthue = Regex.Match(arr.FirstOrDefault(), @"[0-9]+-[0-9]{0,3}").Value;
          else
            vna.ttdk_msthue = Regex.Match(arr.FirstOrDefault(), @"\d+").Value;

          int solanlap1 = 0;
          model.path = linkct;/*update link*/
          getTrangConNew(model, ref vna, ref solanlap1, arrControl);


          if (vna != null && SQLDatabase.AddVinabiz(vna))   /*lưu thông tin vào csdl*/
            _thanhcong++;
          else
            _thatbai++;
          lbl_thanhcong.Invoke((Action)delegate {
            lbl_thanhcong.Text = _thanhcong.ToString("#,##0");
          });
          lbl_thatbai.Invoke((Action)delegate {
            lbl_thatbai.Text = _thatbai.ToString("#,##0");
          });
        }
      }

      catch (Exception e) {
        _thatbai++;
        return;
      }
    }

    public static void getTrangCon(Gridview model, ref vinabiz vina, ref int solanlap, object arrControl) {
      string strWebsite = "";

      ArrayList arr1 = (ArrayList)arrControl;
      Label txtMessage = (Label)arr1[0];
      Label lbl_vinabiz_khoa = (Label)arr1[1];

      List<string> arrGetHtmlDocumentALL;
      List<string> arrGetHtmlDocumentALLKhuVuc;
      int vt = -1;

      try {
        //strPath = "https://vinabiz.org//company/detail/cong-ty-tnhh-cong-nghe-dien-tu-vien-thong-cat-tuong-vi/3100360030003200300034003700310030003200";

        strWebsite = WebRequestNavigate(model.path, ref solanlap, lbl_vinabiz_khoa);

        /*-----------------------------------------------------*/
        int vtkhuvuc1 = strWebsite.IndexOf("class=\"page-title txt-color-blueDark\"");
        int vtkhuvuc2 = strWebsite.IndexOf("id=\"widget-grid\"", vtkhuvuc1);
        string chuoiKhuVuc = strWebsite.Substring(vtkhuvuc1, vtkhuvuc2 - vtkhuvuc1);
        arrGetHtmlDocumentALLKhuVuc = chuoiKhuVuc.Split(new char[] { '\n', '\r' }).ToList().Where(p => p.Contains("a href=") && !p.Contains("Trang chủ") && !p.Contains("Tỉnh Thành")).ToList();

        List<string> listKhuVuc = Helpers.getDataHTML(arrGetHtmlDocumentALLKhuVuc.FirstOrDefault()).Where(p => !p.Contains(">") && p.Length != 0).ToList();

        vina.ttlh_tinh = listKhuVuc[0].Trim();
        vina.ttlh_tinhid = _listTinh.Where(p => p.ten.Trim().Contains(listKhuVuc[0].Trim())).FirstOrDefault().id;
        if (listKhuVuc.Count() == 2)
          vina.ttlh_xa = listKhuVuc[1];
        else if (listKhuVuc.Count() == 3) {
          vina.ttlh_huyen = listKhuVuc[1];
          vina.ttlh_xa = listKhuVuc[2];
        }
        /*------------------------------------------------------*/

        int vt1 = strWebsite.IndexOf("id=\"wid-detail-info\"");
        int vt2 = strWebsite.IndexOf("class=\"page-footer\"", vt1);
        string chuoixuly = strWebsite.Substring(vt1, vt2 - vt1);

        arrGetHtmlDocumentALL = chuoixuly.Split(new char[] { '\n', '\r' }).ToList();

        /*sys_website*/
        vina.web_nguon_url = model.path;
        vina.danhmucid = IdDanhmuc;
        /*detailcompany_name- Tên công ty*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Tên chính thức</td>");
        if (vt != -1) {
          vina.ttdk_tenchinhthuc = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1].ToString()).FirstOrDefault();
        }
        /*tên giao dich*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Tên giao dịch</td>");
        if (vt != -1) {
          vina.ttdk_tengiaodich = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1].ToString()).FirstOrDefault();
        }

        /*Ngày cấp*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Ngày cấp</td>");
        if (vt != -1) {
          vina.ttdk_ngaycap = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1].ToString()).FirstOrDefault();
        }
        /*Cơ quan thuế quản lý*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Cơ quan thuế quản lý</td>");
        if (vt != -1) {
          vina.ttdk_coquanthuequanly = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1].ToString()).FirstOrDefault();
        }
        /*Ngày bắt đầu hoạt động*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Ngày bắt đầu hoạt động</td>");
        if (vt != -1) {
          vina.ttdk_ngaybatdauhoatdong = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1].ToString()).FirstOrDefault();

        }
        /*Trạng thái*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Trạng thái</td>");
        if (vt != -1) {
          vina.ttdk_trangthai = arrGetHtmlDocumentALL[vt + 4].ToString().Replace("<strong></strong>", string.Empty);

        }
        /*Thông tin liên hệ*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Địa chỉ trụ sở</td>");
        if (vt != -1) {
          string strchuoicon = arrGetHtmlDocumentALL[vt + 2].ToString();
          if (strchuoicon.Length > 0) {
            int vtcon1 = strchuoicon.IndexOf('<');
            string strDiaChiTruSo = strchuoicon.Substring(0, vtcon1);
            foreach (var item in Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 2].ToString())) {
              strDiaChiTruSo += " " + item;
            }
            vina.ttlh_diachitruso = strDiaChiTruSo;
          }
        }
        /*Điện thoại*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Điện thoại</td>");
        int vttemp = vt;
        if (vt != -1) {
          string strPhone1 = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 2]).Where(p => !p.Contains("Đăng nhập") && p.Length != 0).FirstOrDefault();
          vina.ttlh_dienthoai1 = strPhone1;
          List<string> arrPhone = Utilities_scanner.getPhoneHTML(strPhone1, _dau_so, _regexs);
          if (arrPhone != null) {
            if (arrPhone.Count() != 0)
              vina.ttlh_dienthoaididong1 = arrPhone.FirstOrDefault();
            if (arrPhone.Count() >= 2)
              vina.ttlh_dienthoaididong2 = arrPhone[1];
            if (arrPhone.Count() >= 3)
              vina.ttlh_dienthoaididong3 = arrPhone[2];
          }
        }
        /*điện thoại người đại diện*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Điện thoại</td>", vttemp);
        if (vt != -1) {
          string strPhone1 = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 3]).Where(p => !p.Contains("Đăng nhập") && p.Length != 0).FirstOrDefault();
          vina.ttlh_dienthoai_nguoidaidien = strPhone1;
          List<string> arrPhone = Utilities_scanner.getPhoneHTML(strPhone1, _dau_so, _regexs);
          if (arrPhone != null && arrPhone.Count() != 0)
            vina.ttlh_dienthoai_nguoidaidien_didong = arrPhone.FirstOrDefault();
        }
        /*email*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Email</td>");
        if (vt != -1) {
          if (arrGetHtmlDocumentALL[vt + 2].ToString() != "</td>") {
            //model.ttlh_email = getDecodeEmail(arrGetHtmlDocumentALL[vt + 2]);
            string strEmail = getDecodeEmail(arrGetHtmlDocumentALL[vt + 2]);
            List<string> arrPhone = Utilities_scanner.getEmail(new List<string>() { strEmail });
            if (arrPhone != null && arrPhone.Count() != 0)
              vina.ttlh_email = arrPhone.FirstOrDefault();
          }
        }
        /*Website*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Website</td>");
        if (vt != -1) {
          vina.ttlh_website = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1]).FirstOrDefault();
        }
        /*Fax*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Fax</td>");
        if (vt != -1) {
          vina.ttlh_fax = Regex.Replace(arrGetHtmlDocumentALL[vt + 2], @"\D", "");
        }
        /*Người đại diện*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Người đại diện</td>");
        if (vt != -1) {
          vina.ttlh_nguoidaidien = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1]).FirstOrDefault();
        }
        /*Địa chỉ người đại diện*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Địa chỉ người đại diện</td>");
        if (vt != -1) {
          vina.ttlh_diachinguoidaidien = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1]).FirstOrDefault();
        }
        /*Giám đốc*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Giám đốc</td>");
        if (vt != -1) {
          vina.ttlh_giamdoc = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1]).FirstOrDefault();
        }
        /*Điện thoại giám đốc*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Điện thoại giám đốc</td>");
        if (vt != -1) {
          string strPhone1 = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 3]).Where(p => !p.Contains("Đăng nhập") && p.Length != 0).FirstOrDefault();
          vina.ttlh_dienthoaigiamdoc = strPhone1;
          List<string> arrPhone = Utilities_scanner.getPhoneHTML(strPhone1, _dau_so, _regexs);
          if (arrPhone != null && arrPhone.Count() != 0)
            vina.ttlh_dienthoaigiamdoc_didong = arrPhone.FirstOrDefault();

        }
        /*Địa chỉ giám đốc*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Địa chỉ giám đốc</td>");
        if (vt != -1) {
          vina.ttlh_diachigiamdoc = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1]).FirstOrDefault();
        }
        /*Kế toán*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Kế toán</td>");
        if (vt != -1) {
          vina.ttlh_ketoan = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1]).FirstOrDefault();
        }
        /*Điện thoại kế toán*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Điện thoại kế toán</td>");
        if (vt != -1) {
          string strPhone1 = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 3]).Where(p => !p.Contains("Đăng nhập") && p.Length != 0).FirstOrDefault();
          vina.ttlh_dienthoaiketoan = strPhone1;
          List<string> arrPhone = Utilities_scanner.getPhoneHTML(strPhone1, _dau_so, _regexs);
          if (arrPhone != null && arrPhone.Count() != 0)
            vina.ttlh_dienthoaiketoan_didong = arrPhone.FirstOrDefault();
        }
        /*Địa chỉ kế toán*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Địa chỉ kế toán</td>");
        if (vt != -1) {
          vina.ttlh_diachiketoan = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1]).FirstOrDefault();

        }
        /*Thông tin ngành nghề, lĩnh vực hoạt động*/
        /*Ngành nghề chính*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Ngành nghề chính</td>");
        if (vt != -1) {
          if (Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1]).FirstOrDefault() != null) {
            string strGoc = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1]).FirstOrDefault();
            dm_hsct dm = getIDKiemTraGanDung(strGoc);
            if (dm == null)
              vina.danhmucid = 0;
            else
              vina.danhmucid = dm.id;
            vina.danhmucbyVnbiz = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1]).FirstOrDefault();
          }
        }
        /*Lĩnh vực kinh tế*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Lĩnh vực kinh tế</td>");
        if (vt != -1) {
          vina.lvhd_linhvuckinhte = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1]).FirstOrDefault();
        }
        /*Loại hình kinh tế*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Loại hình kinh tế</td>");
        if (vt != -1) {
          vina.lvhd_loaihinhkinhte = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1]).FirstOrDefault();
        }
        /*Loại hình tổ chức*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Loại hình tổ chức</td>");
        if (vt != -1) {
          vina.lvhd_loaihinhtochuc = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1]).FirstOrDefault();
        }
        /*Cấp chương*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Cấp chương</td>");
        if (vt != -1) {
          vina.lvhd_capchuong = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1]).FirstOrDefault();
        }
        /*Loại khoản*/
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "<td class=\"bg_table_td\">Loại khoản</td>");
        if (vt != -1) {
          vina.lvhd_loaikhoan = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt + 1]).FirstOrDefault();
        }
        /*Ngành nghề kinh doanh*/
        /*tab-pane fade active in*/
        if (strWebsite.IndexOf("<a data-toggle=\"tab\" href=\"#hr2\">") != -1) {/*có Ngành nghề kinh doanh*/
          int vtnganhnghekd1 = strWebsite.IndexOf("<div class=\"tab-pane fade\" id=\"hr2\">");
          int vtnganhnghekd2 = strWebsite.IndexOf("class=\"widget-footer text-right\"", vtnganhnghekd1);
          string chuoixulynghanhnghe = strWebsite.Substring(vtnganhnghekd1, vtnganhnghekd2 - vtnganhnghekd1);

          List<string> arrHr = chuoixulynghanhnghe.Split(new char[] { '\n', '\r' }).Where(p => p.Contains("tab-pane fade")).ToList();

          int HR2vtnganhnghekd1 = chuoixulynghanhnghe.IndexOf("id=\"hr2\"");
          if (HR2vtnganhnghekd1 != -1) {
            int HR2vtnganhnghekd2 = chuoixulynghanhnghe.IndexOf("id=\"hr3\"", HR2vtnganhnghekd1) == -1 ? chuoixulynghanhnghe.Length :
                                                                                                            chuoixulynghanhnghe.IndexOf("id=\"hr3\"", HR2vtnganhnghekd1);
            string HR2chuoixulynghanhnghe = chuoixulynghanhnghe.Substring(HR2vtnganhnghekd1, HR2vtnganhnghekd2 - HR2vtnganhnghekd1);
            string[] arrNganhNghe = HR2chuoixulynghanhnghe.Split(new char[] { '\n', '\r' }).Where(p => p.Contains("<li>")).ToArray();
            foreach (var item in arrNganhNghe) {
              if (item.Contains("<b>")) {/*ngành nghề chính*/
                vina.nganhnghechinh2 = Helpers.getDataHTML(item).Where(p1 => p1.Trim().Length > 0).FirstOrDefault().Trim();
              }
              vina.ds_nganhnghekinhdoanh += vina.ds_nganhnghekinhdoanh == null ? Helpers.getDataHTML(item).Where(p => p.Trim().Length > 0).ToList().FirstOrDefault() : string.Format(" | {0}", Helpers.getDataHTML(item).Where(p => p.Trim().Length > 0).ToList().FirstOrDefault());
            }
          }
        }
        if (strWebsite.IndexOf("<div class=\"tab-pane fade\" id=\"hr3\">") != -1) {
          int vtnganhnghekd1 = strWebsite.IndexOf("<div class=\"tab-pane fade\" id=\"hr3\">");
          int vtnganhnghekd2 = strWebsite.IndexOf("class=\"widget-footer text-right\"", vtnganhnghekd1);
          string chuoixulythuephainop = strWebsite.Substring(vtnganhnghekd1, vtnganhnghekd2 - vtnganhnghekd1);



          string[] arrNganhNghe = chuoixulythuephainop.Split(new char[] { '\n', '\r' }).Where(p => p.Contains("<li>")).ToArray();
          foreach (var item in arrNganhNghe) {
            vina.ds_thuephainop += vina.ds_thuephainop == null ? Helpers.getDataHTML(item).Where(p => p.Trim().Length > 0).ToList().FirstOrDefault() : string.Format(" | {0}", Helpers.getDataHTML(item).Where(p => p.Trim().Length > 0).ToList().FirstOrDefault());
          }


        }



      }
      catch (Exception e) {
        _thatbai++;
        vina = null;
        writer = LogWriter.Instance;
        writer.WriteToLog(string.Format("{0}-{1}-{2}", e.Message, model.path, "getTrangCon-vinabiz"));
        return;
      }
    }

    public static void getTrangConNew(Gridview model, ref vinabiz vina, ref int solanlap, object arrControl) {
      string strWebsite = "";

      ArrayList arr1 = (ArrayList)arrControl;
      RichTextBox txtMessage = (RichTextBox)arr1[0];
      Label lbl_vinabiz_khoa = (Label)arr1[1];
      int vt = -1;

      try {
        //strPath = "https://vinabiz.org//company/detail/cong-ty-tnhh-phat-trien-dat-xanh/3600300030003100360031003100320033003500";
        strWebsite = WebRequestNavigateNew(model.path, ref solanlap, lbl_vinabiz_khoa);
        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(strWebsite);
        HtmlNode documentNode = doc.DocumentNode;

        string xPath = "//h1[@class='page-title txt-color-blueDark']";
        HtmlNode listNode = documentNode.SelectSingleNode(xPath);
        /*khu vực*/
        string khuvuc = listNode.InnerText;
        List<string> arrkhuvuc = khuvuc.Trim().Split(new char[] { '\n', '\r' }).ToList();
        List<string> listKhuVuc = arrkhuvuc.LastOrDefault().Split('>').ToList();

        vina.ttlh_tinh = listKhuVuc[0].Trim();
        vina.ttlh_tinhid = _listTinh.Where(p => p.ten.Trim().Contains(listKhuVuc[0].Trim())).Count() > 0 ? _listTinh.Where(p => p.ten.Trim().Contains(listKhuVuc[0].Trim())).FirstOrDefault().id : 0;
        if (listKhuVuc.Count() == 2)
          vina.ttlh_xa = listKhuVuc[1];
        else if (listKhuVuc.Count() == 3) {
          vina.ttlh_huyen = listKhuVuc[1];
          vina.ttlh_xa = listKhuVuc[2];
        }
        /*sys_website*/
        vina.web_nguon_url = model.path;
        vina.danhmucid = IdDanhmuc;
        //table table-bordered
        string xPathinfo = "//table[@class='table table-bordered']";
        HtmlNode listNodeinfo = documentNode.SelectSingleNode(xPathinfo);

        List<string> arrThongTinDoanhNghiep;
        arrThongTinDoanhNghiep = listNodeinfo.InnerHtml.Split(new char[] { '\n', '\r' }).ToList();

        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Tên chính thức</td>");
        if (vt != -1) {
          vina.ttdk_tenchinhthuc = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1].ToString()).FirstOrDefault();
        }
        /*tên giao dich*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Tên giao dịch</td>");
        if (vt != -1) {
          vina.ttdk_tengiaodich = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1].ToString()).FirstOrDefault();
        }
        /*Ngày cấp*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Ngày cấp</td>");
        if (vt != -1) {
          vina.ttdk_ngaycap = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1].ToString()).FirstOrDefault();
        }
        /*Cơ quan thuế quản lý*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Cơ quan thuế quản lý</td>");
        if (vt != -1) {
          vina.ttdk_coquanthuequanly = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1].ToString()).FirstOrDefault();
        }
        /*Ngày bắt đầu hoạt động*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Ngày bắt đầu hoạt động</td>");
        if (vt != -1) {
          vina.ttdk_ngaybatdauhoatdong = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1].ToString()).FirstOrDefault();

        }
        /*Trạng thái*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Trạng thái</td>");
        if (vt != -1) {
          vina.ttdk_trangthai = arrThongTinDoanhNghiep[vt + 4].ToString().Replace("<strong></strong>", string.Empty);

        }
        /*Thông tin liên hệ*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Địa chỉ trụ sở</td>");
        if (vt != -1) {
          string strchuoicon = arrThongTinDoanhNghiep[vt + 2].ToString();
          if (strchuoicon.Length > 0) {
            int vtcon1 = strchuoicon.IndexOf('<');
            string strDiaChiTruSo = strchuoicon.Substring(0, vtcon1);
            foreach (var item in Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 2].ToString())) {
              strDiaChiTruSo += " " + item;
            }
            vina.ttlh_diachitruso = strDiaChiTruSo;
          }
        }
        /*Điện thoại*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Điện thoại</td>");
        int vttemp1 = vt;
        if (vt != -1) {
          string strPhone1 = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 2]).Where(p => p.Length != 0).FirstOrDefault();
          vina.ttlh_dienthoai1 = strPhone1;
          List<string> arrPhone = Utilities_scanner.getPhoneHTML(strPhone1, _dau_so, _regexs);
          if (arrPhone != null && arrPhone.Count() != 0) {
            if (arrPhone.Count() != 0)
              vina.ttlh_dienthoaididong1 = arrPhone.FirstOrDefault();
            if (arrPhone.Count() >= 2)
              vina.ttlh_dienthoaididong2 = arrPhone[1];
            if (arrPhone.Count() >= 3)
              vina.ttlh_dienthoaididong3 = arrPhone[2];
          }
        }
        /*điện thoại người đại diện*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Điện thoại</td>", vttemp1);
        if (vt != -1) {
          string strPhone1 = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 2]).Where(p => p.Length != 0).FirstOrDefault();
          vina.ttlh_dienthoai_nguoidaidien = strPhone1;
          List<string> arrPhone = Utilities_scanner.getPhoneHTML(strPhone1, _dau_so, _regexs);
          if (arrPhone != null && arrPhone.Count() != 0)
            vina.ttlh_dienthoai_nguoidaidien_didong = arrPhone.FirstOrDefault();
        }
        /*email*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Email</td>");
        if (vt != -1) {
          if (arrThongTinDoanhNghiep[vt + 2].ToString() != "</td>") {
            //model.ttlh_email = getDecodeEmail(arrGetHtmlDocumentALL[vt + 2]);
            string strEmail = getDecodeEmail(arrThongTinDoanhNghiep[vt + 2]);
            List<string> arrPhone = Utilities_scanner.getEmail(new List<string>() { strEmail });
            if (arrPhone != null && arrPhone.Count() != 0)
              vina.ttlh_email = arrPhone.FirstOrDefault();
          }
        }
        /*Website*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Website</td>");
        if (vt != -1) {
          vina.ttlh_website = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1]).FirstOrDefault();
        }
        /*Fax*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Fax</td>");
        if (vt != -1) {
          vina.ttlh_fax = Regex.Replace(arrThongTinDoanhNghiep[vt + 2], @"\D", "");
        }
        /*Người đại diện*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Người đại diện</td>");
        if (vt != -1) {
          vina.ttlh_nguoidaidien = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1]).FirstOrDefault();
        }
        /*Địa chỉ người đại diện*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Địa chỉ người đại diện</td>");
        if (vt != -1) {
          vina.ttlh_diachinguoidaidien = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1]).FirstOrDefault();
        }
        /*Giám đốc*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Giám đốc</td>");
        if (vt != -1) {
          vina.ttlh_giamdoc = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1]).FirstOrDefault();
        }
        /*Điện thoại giám đốc*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Điện thoại giám đốc</td>");
        if (vt != -1) {
          string strPhone1 = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 2]).Where(p => p.Length != 0).FirstOrDefault();
          vina.ttlh_dienthoaigiamdoc = strPhone1;
          List<string> arrPhone = Utilities_scanner.getPhoneHTML(strPhone1, _dau_so, _regexs);
          if (arrPhone != null && arrPhone.Count() != 0)
            vina.ttlh_dienthoaigiamdoc_didong = arrPhone.FirstOrDefault();

        }
        /*Địa chỉ giám đốc*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Địa chỉ giám đốc</td>");
        if (vt != -1) {
          vina.ttlh_diachigiamdoc = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1]).FirstOrDefault();
        }
        /*Kế toán*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Kế toán</td>");
        if (vt != -1) {
          vina.ttlh_ketoan = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1]).FirstOrDefault();
        }
        /*Điện thoại kế toán*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Điện thoại kế toán</td>");
        if (vt != -1) {
          string strPhone1 = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 2]).Where(p => p.Length != 0).FirstOrDefault();
          vina.ttlh_dienthoaiketoan = strPhone1;
          List<string> arrPhone = Utilities_scanner.getPhoneHTML(strPhone1, _dau_so, _regexs);
          if (arrPhone != null && arrPhone.Count() != 0)
            vina.ttlh_dienthoaiketoan_didong = arrPhone.FirstOrDefault();
        }
        /*Địa chỉ kế toán*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Địa chỉ kế toán</td>");
        if (vt != -1) {
          vina.ttlh_diachiketoan = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1]).FirstOrDefault();

        }
        /*Thông tin ngành nghề, lĩnh vực hoạt động*/
        /*Ngành nghề chính*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Ngành nghề chính</td>");
        if (vt != -1) {
          if (Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1]).FirstOrDefault() != null) {
            string strGoc = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1]).FirstOrDefault();
            dm_hsct dm = getIDKiemTraGanDung(strGoc);
            if (dm == null)
              vina.danhmucid = 0;
            else
              vina.danhmucid = dm.id;
            vina.danhmucbyVnbiz = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1]).FirstOrDefault();
          }
        }
        /*Lĩnh vực kinh tế*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Lĩnh vực kinh tế</td>");
        if (vt != -1) {
          vina.lvhd_linhvuckinhte = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1]).FirstOrDefault();
        }
        /*Loại hình kinh tế*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Loại hình kinh tế</td>");
        if (vt != -1) {
          vina.lvhd_loaihinhkinhte = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1]).FirstOrDefault();
        }
        /*Loại hình tổ chức*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Loại hình tổ chức</td>");
        if (vt != -1) {
          vina.lvhd_loaihinhtochuc = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1]).FirstOrDefault();
        }
        /*Cấp chương*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Cấp chương</td>");
        if (vt != -1) {
          vina.lvhd_capchuong = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1]).FirstOrDefault();
        }
        /*Loại khoản*/
        vt = Helpers.vitritim(arrThongTinDoanhNghiep, "<td class=\"bg_table_td\">Loại khoản</td>");
        if (vt != -1) {
          vina.lvhd_loaikhoan = Helpers.getDataHTML(arrThongTinDoanhNghiep[vt + 1]).FirstOrDefault();
        }
        /***************ngành nghề kinh doanh************************/
        xPathinfo = "//div[@id='hr2']";
        HtmlNode listNodeNganhNgheKinhDoanh = documentNode.SelectSingleNode(xPathinfo);
        if (listNodeNganhNgheKinhDoanh != null)
          foreach (HtmlAgilityPack.HtmlNode item in listNodeNganhNgheKinhDoanh.SelectNodes(".//li")) {
            if (item.OuterHtml.Contains("</b>")) {
              vina.nganhnghechinh2 = item.InnerText.Trim();
            }
            vina.ds_nganhnghekinhdoanh += vina.ds_nganhnghekinhdoanh == null ? item.InnerText.Trim() : string.Format(" | {0}", item.InnerText.Trim());
          }
        /***************Thuế phải nộp*******************************/
        xPathinfo = "//div[@id='hr3']";
        HtmlNode listNodeThuePhaiNop = documentNode.SelectSingleNode(xPathinfo);
        if (listNodeThuePhaiNop != null)
          foreach (HtmlAgilityPack.HtmlNode item in listNodeThuePhaiNop.SelectNodes(".//li"))
            vina.ds_thuephainop += vina.ds_thuephainop == null ? item.InnerText.Trim() : string.Format(" | {0}", item.InnerText.Trim());
      }
      catch (Exception e) {
        _thatbai++;
        vina = null;
        writer = LogWriter.Instance;
        writer.WriteToLog(string.Format("{0}-{1}-{2}", e.Message, model.path, "getTrangCon-vinabiz"));
        return;
      }
    }

    public static dm_hsct getIDKiemTraGanDung(string chuoisosanh) {
      try {
        /*bước 1: xoá ký tự cuối cùng*/
        if (chuoisosanh.Trim().Length == 0) return null;
        chuoisosanh = chuoisosanh.LastIndexOf(';') == chuoisosanh.Length - 1 ? chuoisosanh.Substring(0, chuoisosanh.Length - 1).Trim().ToLower() : chuoisosanh.Trim().ToLower();
        dm_hsct dm = _listHsct.Where(p => p.name.ToLower().Equals(chuoisosanh)).FirstOrDefault();
        if (dm != null) return dm;
        else {
          dm = _listHsct.Where(p => p.name.ToLower().Contains(chuoisosanh)).FirstOrDefault();
          if (dm != null) return dm;
        }
        foreach (var item in _listHsct) {
          if (chuoisosanh.ToLower().Trim().Equals(item.name.ToLower().Trim()))
            return item;
          string chuoidai = "";
          string chuoingan = "";
          if (chuoisosanh.Trim().Length > item.name.Trim().Length) {
            chuoidai = chuoisosanh.ToLower().Trim();
            chuoingan = item.name.ToLower().Trim();
          }
          else {
            chuoidai = item.name.ToLower().Trim();
            chuoingan = chuoisosanh.ToLower().Trim();
          }
          ApproximatString app = new ApproximatString(chuoidai);
          if (app.SoSanh(chuoingan))
            return item;
        }
        /*kiem tra vet can*/
        dm_vinabiz_map map = _listquetcan.Where(p => p.danhmucbyVnbiz.ToLower().Equals(chuoisosanh)).FirstOrDefault();
        if (map != null) {
          dm = new dm_hsct();
          dm.id = map.hosocongtyid;
          dm.name = map.dmhosocongty;
          return dm;
        }
        return null;
      }
      catch (Exception ex) {
        return null;
      }
    }

    public static string getDecodeEmail(string html) {
      try {
        if (html.IndexOf("data-cfemail=") == -1) return "";
        int vtdau = html.IndexOf("data-cfemail=") + 14;
        string a = html.Substring(vtdau, html.Length - vtdau);
        int vtdau1 = a.IndexOf("\">");
        string aa = a.Substring(0, vtdau1);
        var r = Convert.ToInt32(aa.Substring(0, 2), 16);
        string s = "";
        for (int j = 2; j < vtdau1; j += 2) {
          int c = Convert.ToInt32(aa.Substring(j, 2), 16) ^ r;
          s += Convert.ToChar(c);
        }
        return s;
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message, "getDecodeEmail");
        return "";
      }

    }

  }
  public static class Utilities_thitruongsi {
    public static bool hasProcess = true; /*khai bao bien stop*/
    public static int _PathLimit;/*so trang quet gioi han*/
    public static int _lanquetlai;
    public static int _Sleep;
    public static int _Timeout;
    public static Dictionary<string, int> _dau_so;
    public static List<regexs> _regexs;
    public static bool ChkCKiemTraTrung = false;
    public static string _domain = "https://vinabiz.org/";
    public static int IdDanhmuc;
    public static int _thanhcong = 0;
    public static int _thatbai = 0;
    public static List<string> ds_dauso;
    public static List<dm_hsct> _listHsct;
    public static List<dm_Tinh> _listTinh;
    public static WebBrowser _wThiTruongSi;
    public static infoPathmuaban _infoPathmuaban;
    public static string _danhmuc;
    public static int _danhmucid;
    public static int _limit = 48;
    public static MyDownloader _down;

    private static LogWriter writer;
    public static string WebRequestNavigate(string url) {
      try {

        var tasks = new Task<string>[]
        {
                        _down.Download(url),
        };
        Task.WaitAll(tasks);
        string strHTML = tasks[0].Result;
        return strHTML;
      }
      catch (Exception e) {
        return "";
      }
    }

    public static string WebRequestNavigate(string url, ref int solanlap, Label lbl_khoa) {
      try {
        lbl_khoa.Visible = false;
        //MyDownloader mydown = new MyDownloader();
        var tasks = new Task<string>[]
    {
                        _down.Download(url),
    };

        Task.WaitAll(tasks);
        string strHTML = tasks[0].Result;
        return strHTML;
      }
      catch (WebException ex) {
        if (ex.ToString().Contains("time")) {
          if (solanlap <= _lanquetlai && _lanquetlai != -1) {
            solanlap = solanlap + 1;

            lbl_khoa.Visible = true;
            lbl_khoa.Text = string.Format("Đứt Kết Nối : + {0}", solanlap - 1);
            lbl_khoa.Update();

            Thread.Sleep(_Sleep);
            WebRequestNavigate(url, ref solanlap, lbl_khoa);
          }
          else if (_lanquetlai == -1) {
            lbl_khoa.Visible = true;
            lbl_khoa.Text = string.Format("Đứt Kết Nối..");
            lbl_khoa.Update();
            solanlap = 0;
            Thread.Sleep(_Sleep);
            WebRequestNavigate(url, ref solanlap, lbl_khoa);
          }
          else
            _thatbai++;
        }
        return "";
      }
      catch (Exception e) {
        return "";
      }
    }

    public static infoPathmuaban getPageMax(string url, object arrControl) {
      string strHtml = "";

      ArrayList arr1 = (ArrayList)arrControl;
      Label lbl_vinabiz_message1 = (Label)arr1[0];
      Label lbl_vinabiz_message2 = (Label)arr1[1];
      Label lbl_vinabiz_khoa = (Label)arr1[2];

      infoPathmuaban model = new infoPathmuaban();
      try {
        int solanlap = 0;
        strHtml = Utilities_thitruongsi.WebRequestNavigate(url, ref solanlap, lbl_vinabiz_khoa).ToString();

        List<string> arrGetHtmlDocumentALL = strHtml.Split('\n').ToList();
        int vt = 0;
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "title_result_search");
        if (vt != -1) {
          List<string> arr = Helpers.getDataHTML(arrGetHtmlDocumentALL[vt]);
          if (arr.Count() == 3) {
            int soluongtimthay = ConvertType.ToInt(arr[1].Replace(",", ""));
            model.TotalPagingMax = ConvertType.ToInt(System.Math.Ceiling((double)soluongtimthay / _limit));
          }
        }
        return model;

      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message, "getPageNext");
        return model;
      }
    }
    public static void ShowMessage(Label lbl, string chitiet, int thanhcong, int thatbai) {
      lbl.Text = string.Format("Thành công: {0} /Thất bại: {1} \n -> {2}", thanhcong, thatbai, chitiet);
      lbl.Update();
    }
    public static void getwebBrowser(string strPath, ref int solanlap, object arrControl) {
      ArrayList arr1 = (ArrayList)arrControl;
      Label lbl_si_message1 = (Label)arr1[0];
      Label lbl_si_message2 = (Label)arr1[1];
      Label lbl_si_khoa = (Label)arr1[2];
      Label lbl_si_Par = (Label)arr1[3];
      ProgressBar pro_thitruongsi_tv = (ProgressBar)arr1[4];

      string output;
      List<string> arrGetHtmlDocumentALL;
      Dictionary<string, string> dictionary = new Dictionary<string, string>();

      try {
        if (!Utilities_thitruongsi.hasProcess) return;/*stop*/

        output = WebRequestNavigate(strPath, ref solanlap, lbl_si_khoa);
        int vt1 = output.IndexOf("list_product_search");
        int vt2 = output.IndexOf("pagerlv", vt1);
        if (vt2 == -1)
          vt2 = output.IndexOf("block block-tabs", vt1);
        string strProduct = output.Substring(vt1, vt2 - vt1);

        arrGetHtmlDocumentALL = Regex.Split(strProduct, "<LI class=").Where(p => p.Contains("Category")).ToList();

        string strPathLinkCon = "";
        thitruongsi thitruongsi;
        foreach (var item in arrGetHtmlDocumentALL) {
          thitruongsi = new thitruongsi();
          thitruongsi.danhmucid = _danhmucid;
          /**/
          List<string> arrGet = item.Split(new char[] { '\n', '\r' }).ToList();
          /*tieu de*/
          int vt = 0;
          vt = Helpers.vitritim(arrGet, "<H3 class=title_product>");
          if (vt != -1) {
            thitruongsi.tieude = Helpers.getDataHTML(arrGet[vt]).FirstOrDefault();
          }

          /*name_ncc_thumb*/
          vt = Helpers.vitritim(arrGet, "name_ncc_thumb");
          if (vt != -1) {
            if (arrGet[vt].Contains("class=\"icon_vip icon_business\""))
              thitruongsi.loaidoanhnghiep = "doanh nghiệp";
            else if (arrGet[vt].Contains("icon_vip"))
              thitruongsi.loaidoanhnghiep = "vip";
            else
              thitruongsi.loaidoanhnghiep = "--";
            strPathLinkCon = Helpers.getUrlHtml(arrGet[vt]);
            thitruongsi.sys_diachiweb = strPathLinkCon;
          }
          /*price_thumb*/
          vt = Helpers.vitritim(arrGet, "price_thumb");
          if (vt != -1) {
            List<string> arr = Helpers.getDataHTML(arrGet[vt]);
            thitruongsi.gia_tu_den = string.Format("{0}{1}", arr.FirstOrDefault(), arr.LastOrDefault());
          }
          /*Tối thiểu:*/
          vt = Helpers.vitritim(arrGet, "Tối thiểu:");
          if (vt != -1) {
            thitruongsi.toithieu = Helpers.getDataHTML(arrGet[vt]).LastOrDefault();
          }
          //string strUrlChiTiet = Helpers.getUrlHtml2(item).Where(p=>p.Contains("https://thitruongsi.com/")).FirstOrDefault();   /*xu ly linh chi tiet loi sai link \t*/
          //string strPathLinkCon = getFindLinkCon(strUrlChiTiet,lbl_si_khoa);

          getTrangCon(strPathLinkCon, ref thitruongsi, ref solanlap, arrControl);

          if (Utilities_thitruongsi.hasProcess)/*nếu đang chạy mới được phép lưu thông tin, còn đã dừng tiến trình thi không được phép lưu*/
            if (thitruongsi != null && SQLDatabase.Addthitruongsi(thitruongsi))   /*lưu thông tin vào csdl*/
              _thanhcong++;
            else
              _thatbai++;
          ShowMessage(lbl_si_message2, strPathLinkCon, _thanhcong, _thatbai);
        }

      }

      catch (Exception e) {
        return;
      }
    }

    public static string getFindLinkCon(string url, Label lbl_khoa) {
      try {
        int solanlap = 0;
        string strHtml = Utilities_thitruongsi.WebRequestNavigate(url, ref solanlap, lbl_khoa).ToString();
        int vt1 = strHtml.IndexOf("detail_right");
        int vt2 = strHtml.IndexOf("box_related_shop", vt1);
        string strProduct = strHtml.Substring(vt1, vt2 - vt1);

        List<string> arrGetHtmlDocumentALL = strHtml.ToString().Split('\n').ToList();
        string path = "";


        int vt = 0;
        vt = Helpers.vitritim(arrGetHtmlDocumentALL, "Xem thông tin nhà bán sỉ");
        if (vt != -1) {
          path = Helpers.getUrlHtml(arrGetHtmlDocumentALL[vt].Replace("</div>", string.Empty));
        }
        return path;
      }
      catch (Exception ex) {
        return "";
      }
    }
    public static void getTrangCon(string strPath, ref thitruongsi model, ref int solanlap, object arrControl) {
      string strWebsite = "";

      ArrayList arr1 = (ArrayList)arrControl;
      Label lbl_vinabiz_message1 = (Label)arr1[0];
      Label lbl_vinabiz_message2 = (Label)arr1[1];
      Label lbl_vinabiz_khoa = (Label)arr1[2];

      int vt = -1;

      try {
        // strPath = "https://thitruongsi.com/shop/linh-kien-hung-phat";
        strWebsite = WebRequestNavigate(strPath, ref solanlap, lbl_vinabiz_khoa);
        /*-----------------------------------------------------*/
        int vtkhuvuc1 = strWebsite.IndexOf("info_veryfi_content");
        if (vtkhuvuc1 != -1) {
          int vtkhuvuc2 = strWebsite.IndexOf("header_table_verified", vtkhuvuc1);
          string chuoiKhuVuc = strWebsite.Substring(vtkhuvuc1, vtkhuvuc2 - vtkhuvuc1);
          List<string> arrGetHtmlDocumentALLKhuVuc = chuoiKhuVuc.Split(new char[] { '\n', '\r' }).ToList();

          // model.sys_diachiweb = strPath;
          /*Mô hình kinh doanh:*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALLKhuVuc, "Mô hình kinh doanh:");
          if (vt != -1) {
            model.mohinhkinhdoanh = Helpers.getDataHTML(arrGetHtmlDocumentALLKhuVuc[vt + 4].ToString()).FirstOrDefault();
          }
          /*Tên doanh nghiệp:*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALLKhuVuc, "Tên doanh nghiệp:");
          if (vt != -1) {
            model.tendoanhnghiep = Helpers.getDataHTML(arrGetHtmlDocumentALLKhuVuc[vt + 4].ToString()).FirstOrDefault();
          }
          /*Thời gian hoạt động :*/
          vt = Helpers.vitritim(arrGetHtmlDocumentALLKhuVuc, "Thời gian hoạt động :");
          if (vt != -1) {
            model.thoigianhoatdong = Helpers.getDataHTML(arrGetHtmlDocumentALLKhuVuc[vt + 4].ToString()).FirstOrDefault();
          }
          /**/
          int vt1 = strWebsite.IndexOf("Thông tin doanh nghiệp");
          int vt2 = strWebsite.IndexOf("info-verified", vt1);
          string chuoichung = strWebsite.Substring(vt1, vt2 - vt1);
          List<string> arrThongTindoanhNghiep = chuoichung.Split(new char[] { '\n', '\r' }).ToList();
          /*Mã số thuế:*/
          vt = Helpers.vitritim(arrThongTindoanhNghiep, "Mã số thuế:");
          if (vt != -1) {
            model.ttdn_msthue = Helpers.getDataHTML(arrThongTindoanhNghiep[vt + 2].ToString()).FirstOrDefault();
          }
          /**********************************************************/
          int vtdd1 = strWebsite.IndexOf("Đại diện ");
          int vtdd2 = strWebsite.IndexOf("info-verified", vtdd1);
          string chuoidaidien = strWebsite.Substring(vtdd1, vtdd2 - vtdd1);
          List<string> arrdaidien = chuoidaidien.Split(new char[] { '\n', '\r' }).ToList();
          vt = Helpers.vitritim(arrdaidien, "Họ tên:");
          if (vt != -1) {
            model.daidien_hoten = Helpers.getDataHTML(arrdaidien[vt + 2].ToString()).FirstOrDefault();
          }
        }
        /**********************************************************/
        int vtlhkd1 = strWebsite.IndexOf("Liên hệ phòng kinh doanh");
        if (vtlhkd1 != -1) {
          int vtlhkd2 = strWebsite.IndexOf("</TABLE>", vtlhkd1);
          string lienhekinhdoanh = strWebsite.Substring(vtlhkd1, vtlhkd2 - vtlhkd1);
          List<string> arrlienhekinhdoanh = lienhekinhdoanh.Split(new char[] { '\n', '\r' }).Where(p => p.Contains("<TD>")).ToList();
          if (arrlienhekinhdoanh.Count() == 2) {
            model.lienhe_kinhdoanh_hoten1 = Helpers.getDataHTML(arrlienhekinhdoanh[0].ToString()).FirstOrDefault().Replace(":", "");
            model.lienhe_kinhdoanh_sodienthoai1 = Helpers.getDataHTML(arrlienhekinhdoanh[1].ToString()).FirstOrDefault();
            List<string> arr = Utilities_scanner.getPhoneHTML(model.lienhe_kinhdoanh_sodienthoai1, _dau_so, _regexs);
            if (arr != null)
              model.lienhe_kinhdoanh_sodienthoaibydidong1 = arr.FirstOrDefault();
          }
          else if (arrlienhekinhdoanh.Count() == 4) {
            model.lienhe_kinhdoanh_hoten1 = Helpers.getDataHTML(arrlienhekinhdoanh[0].ToString()).FirstOrDefault().Replace(":", "");
            model.lienhe_kinhdoanh_sodienthoai1 = Helpers.getDataHTML(arrlienhekinhdoanh[1].ToString()).FirstOrDefault();
            List<string> arr = Utilities_scanner.getPhoneHTML(model.lienhe_kinhdoanh_sodienthoai1, _dau_so, _regexs);
            if (arr.Count() > 0)
              model.lienhe_kinhdoanh_sodienthoaibydidong1 = arr.FirstOrDefault();

            model.lienhe_kinhdoanh_hoten2 = Helpers.getDataHTML(arrlienhekinhdoanh[2].ToString()).FirstOrDefault().Replace(":", "");
            model.lienhe_kinhdoanh_sodienthoai2 = Helpers.getDataHTML(arrlienhekinhdoanh[3].ToString()).FirstOrDefault();
            List<string> arr2 = Utilities_scanner.getPhoneHTML(model.lienhe_kinhdoanh_sodienthoai2, _dau_so, _regexs);
            if (arr2 != null)
              model.lienhe_kinhdoanh_sodienthoaibydidong2 = arr2.FirstOrDefault();
          }
        }
        /*********************************************************/
        //Quy mô sản xuất
        int vtQM1 = strWebsite.IndexOf("Quy mô sản xuất");
        if (vtQM1 != -1) {
          int vtQM2 = strWebsite.IndexOf("clear", vtQM1);
          string strqm = strWebsite.Substring(vtQM1, vtQM2 - vtQM1);
          List<string> arrQM = strqm.Split(new char[] { '\n', '\r' }).Where(p => p.Contains("<TD>")).ToList();
          if (arrQM.Count() == 4) {
            model.quymosx_thitruong = Helpers.getDataHTML(arrQM[0].ToString()).FirstOrDefault();
            model.quymosx_nganhhang = Helpers.getDataHTML(arrQM[1].ToString()).FirstOrDefault();
            model.quymosx_nhansu = Helpers.getDataHTML(arrQM[2].ToString()).FirstOrDefault();
            model.quymosx_sanluong = Helpers.getDataHTML(arrQM[3].ToString()).FirstOrDefault();
          }
        }
        /************************************************************/
        //Kho hàng
        int vtKH1 = strWebsite.IndexOf("Kho hàng");
        if (vtKH1 != -1) {
          int vtKH2 = strWebsite.IndexOf("clear", vtKH1);
          string strKH = strWebsite.Substring(vtKH1, vtKH2 - vtKH1);
          List<string> arrKH = strKH.Split(new char[] { '\n', '\r' }).Where(p => p.Contains("<TD>")).ToList();
          if (arrKH.Count() == 1) {
            model.khohang_diachi = Helpers.getDataHTML(arrKH[0].ToString()).FirstOrDefault();
          }
        }
        /************************************************************/
        //Cửa hàng
        int vtCH1 = strWebsite.IndexOf("Kho hàng");
        if (vtCH1 != -1) {
          int vtCH2 = strWebsite.IndexOf("clear", vtCH1);
          string strCH = strWebsite.Substring(vtCH1, vtCH2 - vtCH1);
          List<string> arrCH = strCH.Split(new char[] { '\n', '\r' }).Where(p => p.Contains("<TD>")).ToList();
          if (arrCH.Count() == 1) {
            model.cuahang_diachi = Helpers.getDataHTML(arrCH[0].ToString()).FirstOrDefault();
          }
        }
        /************************************************************/
        //Hình ảnh xưởng
        int vtHA1 = strWebsite.IndexOf("Hình ảnh xưởng");
        if (vtHA1 != -1) {
          int vtHA2 = strWebsite.IndexOf("wrap_footer", vtHA1);
          string strHA = strWebsite.Substring(vtHA1, vtHA2 - vtHA1);
          List<string> arrHA = strHA.Split(new char[] { '\n', '\r' }).Where(p => p.Contains("<TD>")).ToList();
          if (arrHA.Count() == 1) {
            model.hinhanhxuong_diachi = Helpers.getDataHTML(arrHA[0].ToString()).FirstOrDefault();
          }
        }
        /***********************************************************/
        /*tim theo https://mcdn.thitruongsi.com/images/icon_ncc.png*/
        //filter_ncc_seach
        int vtCC1 = strWebsite.IndexOf("left_page_search left_mini_site");
        if (vtCC1 != -1) {
          int vtCC2 = strWebsite.IndexOf("wrap_footer", vtCC1);
          string strCC = strWebsite.Substring(vtCC1, vtCC2 - vtCC1);
          List<string> arrCC = strCC.Split(new char[] { '\n', '\r' }).ToList();
          /*fa-map-signs*/
          vt = Helpers.vitritim(arrCC, "fa-map-signs");
          if (vt != -1) {
            List<string> arr11 = Regex.Split(arrCC[vt].ToString(), "</I>").ToList();
            model.ttncc_diachi = arr11.LastOrDefault();
          }
          /*fa-phone-square*/
          vt = Helpers.vitritim(arrCC, "fa-phone-square");
          if (vt != -1) {
            List<string> arr11 = Regex.Split(arrCC[vt].ToString(), "</I>").ToList();
            model.ttncc_sdt = arr11.LastOrDefault();

            List<string> arr = Utilities_scanner.getPhoneHTML(arr11.LastOrDefault(), _dau_so, _regexs);
            if (arr.Count() > 0)
              model.ttncc_sdt_didong = arr.FirstOrDefault();
          }
          /*fa-envelope*/
          vt = Helpers.vitritim(arrCC, "fa-envelope");
          if (vt != -1) {
            List<string> arr = Helpers.getDataHTML(arrCC[vt].ToString());
            if (arr.Count() > 0)
              model.ttncc_email = arr.FirstOrDefault();
          }
          /*co giay phep kinh doanh*/
          vt = Helpers.vitritim(arrCC, "https://mcdn.thitruongsi.com/images/icon_giayphep.png");
          if (vt != -1) {

            model.daxacthuc_cogiayphepkinhdoanh = Helpers.getDataHTML(arrCC[vt].ToString()).LastOrDefault();
          }
          /*co kho hang*/
          vt = Helpers.vitritim(arrCC, "https://mcdn.thitruongsi.com/images/icon_khohang.png");
          if (vt != -1) {
            model.daxacthuc_cokhohang = Helpers.getDataHTML(arrCC[vt].ToString()).LastOrDefault();
          }
        }
        /*danh muc*/
        //panel-collapse collapse in
        /*https://mcdn.thitruongsi.com/images/icon_cate.png*/
        int vtdm1 = strWebsite.IndexOf("panel-collapse collapse in");
        if (vtdm1 != -1) {
          int vtdm2 = strWebsite.IndexOf("wrap_footer", vtdm1);
          string strDM = strWebsite.Substring(vtdm1, vtdm2 - vtdm1);
          List<string> arrDM = strDM.Split(new char[] { '\n', '\r' }).Where(p => p.Contains("li_cate_lv1")).ToList();
          string strDanhSach = "";
          foreach (var item in arrDM) {
            strDanhSach += string.Format("{0} |", Helpers.getDataHTML(item).FirstOrDefault());
          }
          model.listdanhmuc = strDanhSach;
        }

      }
      catch (Exception e) {
        _thatbai++;
        model = null;
        writer = LogWriter.Instance;
        writer.WriteToLog(string.Format("{0}-{1}-{2}", e.Message, strPath, "getTrangCon-vinabiz"));
        return;
      }
    }
    /*
    public static dm_hsct getIDKiemTraGanDung(string chuoisosanh)
    {
        try
        {
            foreach (var item in _listHsct)
            {
                if (chuoisosanh.ToLower().Trim().Equals(item.name.ToLower().Trim()))
                    return item;
                string chuoidai = "";
                string chuoingan = "";
                if (chuoisosanh.Trim().Length > item.name.Trim().Length)
                {
                    chuoidai = chuoisosanh.ToLower().Trim();
                    chuoingan = item.name.ToLower().Trim();
                }
                else
                {
                    chuoidai = item.name.ToLower().Trim();
                    chuoingan = chuoisosanh.ToLower().Trim();
                }
                ApproximatString app = new ApproximatString(chuoidai);
                if (app.SoSanh(chuoingan))
                    return item;
            }
            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    */
    public static string getDecodeEmail(string html) {
      try {
        if (html.IndexOf("data-cfemail=") == -1) return "";
        int vtdau = html.IndexOf("data-cfemail=") + 14;
        string a = html.Substring(vtdau, 100);
        int vtdau1 = a.IndexOf("\">");
        string aa = a.Substring(0, vtdau1);
        var r = Convert.ToInt32(aa.Substring(0, 2), 16);
        string s = "";
        for (int j = 2; j < vtdau1; j += 2) {
          int c = Convert.ToInt32(aa.Substring(j, 2), 16) ^ r;
          s += Convert.ToChar(c);
        }
        return s;
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message, "getDecodeEmail");
        return "";
      }

    }

  }

  public class ConvertType {
    public static int ToInt(object obj) {
      try {
        if (obj == null)
          return 0;
        int rs = System.Convert.ToInt32(obj);
        if (rs < 0)
          return 0;
        return rs;
      }
      catch {
        return 0;
      }
    }
    public static double ToDouble(object obj) {
      try {
        if (obj == null)
          return 0;
        double rs = System.Convert.ToDouble(obj);
        if (rs < 0)
          return 0;
        return rs;
      }
      catch {
        return 0;
      }
    }
    public static decimal ToDecimal(object obj) {
      try {
        if (obj == null)
          return 0;
        decimal rs = System.Convert.ToDecimal(obj);
        if (rs < 0)
          rs = 0;
        return rs;
      }
      catch { return 0; }
    }
    public static string ToString(object obj) {
      try {
        if (obj == null)
          return "";
        return System.Convert.ToString(obj);
      }
      catch {
        return "";
      }
    }
    public static float ToFloat(object obj) {
      try {
        if (obj == null)
          return 0;
        float rs = float.Parse(obj.ToString());
        if (rs < 0)
          return 0;
        return rs;
      }
      catch {
        return 0;
      }
    }
    public static DateTime ToDateTime(object obj) {
      try {
        if (obj == null)
          return DateTime.Now;
        DateTime dt = System.Convert.ToDateTime(obj, System.Globalization.CultureInfo.InvariantCulture);

        return dt;
      }
      catch {
        return DateTime.Now;
      }
    }
    public static Guid ToGuid(object obj) {
      try {
        if (obj == null)
          return Guid.Empty;
        Guid dt = new Guid(obj.ToString());
        return dt;
      }
      catch {
        return Guid.Empty;
      }
    }

  }
  public class Export {
    public static void ExportText(DataTable table1, string FileName, string kytu) {


      System.IO.StreamWriter str = new System.IO.StreamWriter(FileName);


      if (table1.Rows.Count == 0) {
        MessageBox.Show("Không có dữ liệu import", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }


      foreach (DataRow item in table1.Rows) {
        int iz = 0;
        string strLine = "";
        foreach (var item1 in table1.Columns) {
          if (iz != 0)
            //strLine += ";";
            strLine += kytu;
          strLine += item[iz].ToString();
          iz++;
        }
        str.WriteLine(strLine);
      }
      str.Flush();
      str.Close();
    }

    public class ExcelAdapter {
      protected string sFilePath;
      public string SFilePath {
        get { if (sFilePath == null) return ""; return sFilePath; }
        set { sFilePath = value; }
      }

      public ExcelAdapter(string filePath) {
        this.SFilePath = filePath;
      }

      public bool DeleteFile() {
        if (File.Exists(this.SFilePath)) {
          File.Delete(this.SFilePath);
          return true;
        }
        else
          return false;
      }

      public bool IsExist() {
        return File.Exists(this.SFilePath);
      }

      public DataTable ReadFromFile(string commandText) {
        string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + this.sFilePath + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";



        DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");

        DbDataAdapter adapter = factory.CreateDataAdapter();

        DbCommand selectCommand = factory.CreateCommand();
        selectCommand.CommandText = commandText;

        DbConnection connection = factory.CreateConnection();
        connection.ConnectionString = connectionString;

        selectCommand.Connection = connection;

        adapter.SelectCommand = selectCommand;

        DataSet cities = new DataSet();

        adapter.Fill(cities);

        connection.Close();
        adapter.Dispose();

        return cities.Tables[0];
      }

      protected void FormatDate(COMExcel.Worksheet sheet, int rstart, int cstart, int rend, int cend) {
        COMExcel.Range range = (COMExcel.Range)sheet.Range[sheet.Cells[rstart, cstart], sheet.Cells[rend, cend]];
        range.NumberFormat = "DD/MM/YYYY";
      }

      protected void FormatMoney(COMExcel.Worksheet sheet, int rstart, int cstart, int rend, int cend) {
        COMExcel.Range range = (COMExcel.Range)sheet.Range[sheet.Cells[rstart, cstart], sheet.Cells[rend, cend]];
        range.NumberFormat = "#,##0";
      }

      protected void Format(COMExcel.Worksheet sheet, int rstart, int cstart, int rend, int cend, string type) {
        COMExcel.Range range = (COMExcel.Range)sheet.Range[sheet.Cells[rstart, cstart], sheet.Cells[rend, cend]];
        range.NumberFormat = type;
      }

      public string CreateAndWrite(DataTable dt, string sheetName, int noSheet) {
        using (new ExcelUILanguageHelper()) {
          COMExcel.Application exApp = new COMExcel.Application();
          COMExcel.Workbook exBook = exApp.Workbooks.Add(
                        COMExcel.XlWBATemplate.xlWBATWorksheet);
          try {
            // Không hiển thị chương trình excel
            exApp.Visible = false;

            // Lấy sheet 1.
            COMExcel.Worksheet exSheet = (COMExcel.Worksheet)exBook.Worksheets[noSheet];
            exSheet.Name = sheetName;

            //////////////////////
            int rowCount = dt.Rows.Count;
            int colCount = dt.Columns.Count;

            // insert header name             
            for (int j = 1; j <= colCount; j++) {
              exSheet.Cells[1, j] = dt.Columns[j - 1].Caption;
            }

            // format cho header
            COMExcel.Range headr = (COMExcel.Range)exSheet.Range[exSheet.Cells[1, 1], exSheet.Cells[1, colCount]];
            headr.Interior.Color = System.Drawing.Color.Gray.ToArgb();
            headr.Font.Bold = true;
            headr.Font.Name = "Arial";
            headr.Font.Color = System.Drawing.Color.White.ToArgb();
            headr.Cells.RowHeight = 30;
            headr.Cells.ColumnWidth = 20;
            headr.HorizontalAlignment = COMExcel.Constants.xlCenter;


            //format cho cot ngay, tien, so
            for (int i = 1; i <= colCount; i++) {
              if (dt.Columns[i - 1].DataType == Type.GetType("System.DateTime")) {
                FormatDate(exSheet, 2, i, rowCount + 1, i);
              }
              else if (dt.Columns[i - 1].DataType == Type.GetType("System.Decimal")) {
                Format(exSheet, 2, i, rowCount + 1, i, "##0.0");
              }
              else if (dt.Columns[i - 1].DataType == Type.GetType("System.Int64")) {
                FormatMoney(exSheet, 2, i, rowCount + 1, i);
              }
              else if (dt.Columns[i - 1].DataType == Type.GetType("System.Int32")) {
              }
              else {
                Format(exSheet, 2, i, rowCount + 1, i, "@");
              }
            }
            for (int i = 1; i <= rowCount; i++) {
              for (int j = 1; j <= colCount; j++) {
                exSheet.Cells[i + 1, j] = dt.Rows[i - 1][j - 1].ToString();
              }
            }

            //format cho toan bo sheet
            COMExcel.Range Sheet = (COMExcel.Range)exSheet.Range[exSheet.Cells[1, 1], exSheet.Cells[rowCount + 1, colCount]];
            Sheet.Borders.Color = System.Drawing.Color.Black.ToArgb();
            Sheet.WrapText = false;

            // Save file
            exBook.SaveAs(this.SFilePath, COMExcel.XlFileFormat.xlWorkbookNormal,
                            null, null, false, false,
                            COMExcel.XlSaveAsAccessMode.xlExclusive,
                            false, false, false, false, false);


            return "Export file excel thành công.\nĐường dẫn là: " + this.sFilePath;
          }
          catch (Exception ex) {
            Thread.CurrentThread.CurrentCulture.DateTimeFormat = new System.Globalization.CultureInfo("en-US").DateTimeFormat;
            return ex.ToString();
          }
          finally {
            Thread.CurrentThread.CurrentCulture.DateTimeFormat = new System.Globalization.CultureInfo("en-US").DateTimeFormat;
            // Đóng chương trình
            exBook.Close(false, false, false);
            exApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(exBook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(exApp);
          }
        }
      }

      public string CreateAndWrite(DataTable[] dtList, string[] sheetNames) {
        using (new ExcelUILanguageHelper()) {
          COMExcel.Application exApp = new COMExcel.Application();
          COMExcel.Workbook exBook = exApp.Workbooks.Add(
                        COMExcel.XlWBATemplate.xlWBATWorksheet);
          try {
            // Không hiển thị chương trình excel
            exApp.Visible = false;

            //List<COMExcel.Worksheet> exSheetList = new List<Microsoft.Office.Interop.Excel.Worksheet>();
            for (int i = 1; i < dtList.Length; i++) {
              //exSheetList.Add((COMExcel.Worksheet)exBook.Worksheets[i]);
              exBook.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            int noSheet = 1;
            foreach (DataTable dt in dtList) {
              COMExcel.Worksheet exSheet = (COMExcel.Worksheet)exBook.Worksheets[noSheet];
              exSheet.Name = sheetNames[noSheet - 1];

              //////////////////////
              int rowCount = dt.Rows.Count;
              int colCount = dt.Columns.Count;

              // insert header name             
              for (int j = 1; j <= colCount; j++) {
                exSheet.Cells[1, j] = dt.Columns[j - 1].Caption;
              }

              // format cho header
              COMExcel.Range headr = (COMExcel.Range)exSheet.Range[exSheet.Cells[1, 1], exSheet.Cells[1, colCount]];
              headr.Interior.Color = System.Drawing.Color.Gray.ToArgb();
              headr.Font.Bold = true;
              headr.Font.Name = "Arial";
              headr.Font.Color = System.Drawing.Color.White.ToArgb();
              headr.Cells.RowHeight = 30;
              headr.Cells.ColumnWidth = 20;
              headr.HorizontalAlignment = COMExcel.Constants.xlCenter;


              //format cho cot ngay, tien, so
              for (int i = 1; i <= colCount; i++) {
                if (dt.Columns[i - 1].DataType == Type.GetType("System.DateTime")) {
                  FormatDate(exSheet, 2, i, rowCount + 1, i);
                }
                else if (dt.Columns[i - 1].DataType == Type.GetType("System.Decimal")) {
                  Format(exSheet, 2, i, rowCount + 1, i, "##0.0");
                }
                else if (dt.Columns[i - 1].DataType == Type.GetType("System.Int64")) {
                  FormatMoney(exSheet, 2, i, rowCount + 1, i);
                }
                else if (dt.Columns[i - 1].DataType == Type.GetType("System.Int32")) {
                }
                else {
                  Format(exSheet, 2, i, rowCount + 1, i, "@");
                }
              }

              for (int i = 1; i <= rowCount; i++) {
                for (int j = 1; j <= colCount; j++) {
                  exSheet.Cells[i + 1, j] = dt.Rows[i - 1][j - 1].ToString();
                }
              }

              //format cho toan bo sheet
              COMExcel.Range Sheet = (COMExcel.Range)exSheet.Range[exSheet.Cells[1, 1], exSheet.Cells[rowCount + 1, colCount]];
              Sheet.Borders.Color = System.Drawing.Color.Black.ToArgb();
              Sheet.WrapText = false;

              noSheet++;
            }
            // Save file
            exBook.SaveAs(this.SFilePath, COMExcel.XlFileFormat.xlWorkbookNormal,
                            null, null, false, false,
                            COMExcel.XlSaveAsAccessMode.xlExclusive,
                            false, false, false, false, false);


            return "Export file excel thành công.\nĐường dẫn là: " + this.sFilePath;
          }
          catch (Exception ex) {
            Thread.CurrentThread.CurrentCulture.DateTimeFormat = new System.Globalization.CultureInfo("en-US").DateTimeFormat;
            return ex.ToString();
          }
          finally {
            Thread.CurrentThread.CurrentCulture.DateTimeFormat = new System.Globalization.CultureInfo("en-US").DateTimeFormat;
            // Đóng chương trình
            exBook.Close(false, false, false);
            exApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(exBook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(exApp);
          }
        }
      }

      public class ExcelUILanguageHelper : IDisposable {
        private System.Globalization.CultureInfo m_CurrentCulture;

        public ExcelUILanguageHelper() {
          // save current culture and set culture to en-US            
          Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
          m_CurrentCulture = Thread.CurrentThread.CurrentCulture;
          m_CurrentCulture.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
        }

        #region IDisposable Members

        public void Dispose() {
          // return to normal culture
          Thread.CurrentThread.CurrentCulture = m_CurrentCulture;
        }

        #endregion
      }

    }

    public static bool ExportXml(List<myXML> myxml, string FileName, string KeyFile, int SoTrang, string IndexDM) {
      try {

        XmlTextWriter writer = new XmlTextWriter(FileName, System.Text.Encoding.UTF8);
        writer.WriteStartDocument(true);
        //writer.Formatting = Formatting.Indented;
        writer.Indentation = 2;
        writer.WriteStartElement("Table");

        writer.WriteStartElement("System");
        writer.WriteStartElement("KeyFile");
        writer.WriteString(KeyFile);
        writer.WriteEndElement();

        writer.WriteStartElement("SoTrang");
        writer.WriteString(SoTrang.ToString());
        writer.WriteEndElement();

        writer.WriteStartElement("IndexDM");
        writer.WriteString(IndexDM);
        writer.WriteEndElement();
        writer.WriteEndElement();

        foreach (myXML item in myxml) {
          createNode(item.id, item.name, item.parentid, item.path, item.ma, item.cap, item.orderid, writer);
        }
        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Close();
        return true;

      }
      catch (Exception ex) {

        MessageBox.Show(ex.Message, "ExportXml");
        return false;
      }
    }
    private static void createNode(string pID, string pName, string pParentid, string pPath, string pMa, string pCap, string pOrderid, XmlTextWriter writer) {
      writer.WriteStartElement("AutoLoad");

      writer.WriteStartElement("id");
      writer.WriteString(pID);
      writer.WriteEndElement();

      writer.WriteStartElement("name");
      writer.WriteString(pName);
      writer.WriteEndElement();

      writer.WriteStartElement("path");
      writer.WriteString(pPath);
      writer.WriteEndElement();

      writer.WriteStartElement("ma");
      writer.WriteString(pMa);
      writer.WriteEndElement();

      writer.WriteStartElement("parentid");
      writer.WriteString(pParentid);
      writer.WriteEndElement();

      writer.WriteStartElement("cap");
      writer.WriteString(pCap);
      writer.WriteEndElement();

      writer.WriteStartElement("OrderId");
      writer.WriteString(pOrderid);
      writer.WriteEndElement();

      writer.WriteEndElement();
    }
  }

  public class Ref<T> {
    public Ref() { }
    public Ref(T value) { Value = value; }
    public T Value { get; set; }
    public override string ToString() {
      T value = Value;
      return value == null ? "" : value.ToString();
    }
    public static implicit operator T(Ref<T> r) { return r.Value; }
    public static implicit operator Ref<T>(T value) { return new Ref<T>(value); }
  }

}
