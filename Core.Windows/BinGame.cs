using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using Core.UsuallyCommon;
using System.Linq;

namespace Core.Windows
{
    public partial class BinGame : Form
    {
        public List<Cookies> cookies { get; set; } = new List<Cookies>();
        public BinGame()
        {
            InitializeComponent();
        }
        public ChromiumWebBrowser browser;

        private void BinGame_Load(object sender, EventArgs e)
        {
            Cef.Initialize(new CefSettings());
            browser = new ChromiumWebBrowser("https://www.vip337.com:8899/");
            browser.FrameLoadEnd += Browser_FrameLoadEnd;
            this.groupbwrow.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            var cookieManager = Cef.GetGlobalCookieManager();
            CookieVisitor visitor = new CookieVisitor();
            visitor.SendCookie += visitor_SendCookie;
            cookieManager.VisitAllCookies(visitor);
        }


        public void GetResult()
        {
            var key = "SESSION_ID";
            var value = cookies.FirstOrDefault(x => x.CookieName == key).Value.ToStringExtension().Trim();
            var url = "https://videoley.com/ipl/portal.php/game/betrecord_search/kind3?GameCode=1&GameType=3001&sid=SESSION_ID&lang=cn&rnd1588840209479";

            url = url.Replace(key, value);
            var resulthtml = HttpClientHelper.GetAsync(url, GetCookieContainer());

            HtmlAgilityPack.HtmlDocument docs = new HtmlAgilityPack.HtmlDocument();

            docs.LoadHtml(resulthtml);
            var table = docs.DocumentNode.SelectSingleNode("//*[@class=\"table table-hover text-middle table-bordered footable\"]");
            var trs = table.SelectSingleNode("tbody").SelectNodes("tr").ToList().Take(5);
            if (trs == null)
                return null;
            foreach (var item in trs)
            {
                var tds = item.SelectNodes("td");
                GameResult sx = new GameResult()
                {
                    InvestTime = (tds[0].InnerText).ToDateTime().AddHours(12),
                    OrderNumber = (tds[1].InnerText).ToStringExtension().Trim(),
                    JuHao = (tds[2].InnerText).ToStringExtension().Trim(),
                    ChangCi = (tds[3].InnerText).ToStringExtension().Trim(),
                    GameType = (tds[4].InnerText).ToStringExtension().Trim(),
                    Result = (tds[6].InnerText).ToStringExtension().Trim(),
                    InvestMoney = (tds[7].InnerText).ToStringExtension().Trim().ToDecimal(),
                    ValidMoney = (tds[8].InnerText).ToStringExtension().Trim().ToDecimal(),
                    WinMoney = (tds[9].InnerText).ToStringExtension().Trim().ToDecimal(),
                    Remark = (tds[10].InnerText).ToStringExtension().Trim(),
                };
                games.Add(sx);
            }
        }

        public CookieContainer GetCookieContainer()
        {
            var cookcontainer = new CookieContainer();

            cookies.ForEach(x =>
            cookcontainer.Add(new System.Net.Cookie() { Name = x.CookieName, Value = x.Value, Domain = x.Domain }));

            return cookcontainer;
        }

        private void visitor_SendCookie(CefSharp.Cookie obj)
        {
            cookies.Add(
                new Cookies()
                {
                    CookieName = obj.Name,
                    Value = obj.Value,
                    Domain = obj.Domain,
                    Path = obj.Path
                }
                );
        }
    }

    public class GameResult
    {
        /// <summary>
        /// 投注时间
        /// </summary>
        public DateTime? InvestTime { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 局号
        /// </summary>
        public string JuHao { get; set; }
        /// <summary>
        /// 场次
        /// </summary>
        public string ChangCi { get; set; }
        /// <summary>
        /// 游戏类型
        /// </summary>
        public string GameType { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 投注金额
        /// </summary>
        public Nullable<decimal> InvestMoney { get; set; }
        /// <summary>
        /// 有效金额
        /// </summary>
        public Nullable<decimal> ValidMoney { get; set; }
        /// <summary>
        /// 输赢
        /// </summary>
        public decimal WinMoney { get; set; }
        /// <summary>
        /// 资金备注
        /// </summary>
        public string Remark { get; set; }
    }

    public class CookieVisitor : CefSharp.ICookieVisitor
    {
        public event Action<CefSharp.Cookie> SendCookie;

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public bool Visit(CefSharp.Cookie cookie, int count, int total, ref bool deleteCookie)
        {
            deleteCookie = false;
            if (SendCookie != null)
            {
                SendCookie(cookie);
            }

            return true;
        }
    }
}
