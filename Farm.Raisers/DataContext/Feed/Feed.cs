using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Farm.Raisers.Feeds
{
    public partial class Feed
    {
        private double _nValue;
        private double _unit = 40;
       
        /// <summary>
        /// 饲料名
        /// </summary>
        public string name { get; set; } 

        /// <summary>
        /// 重量(公斤)
        /// </summary>
        public double wValue { get; set; } 

        /// <summary>
        /// 饲料包数
        /// </summary>
        public double nValue 
        { 
            get
            {
                if (_nValue > 0)
                    return _nValue;
                return (double)Math.Round((wValue / _unit), 2);
            }
            set 
            {
                _nValue = value;
            }
        }

        public double exceed
        {
            get { return Math.Round(nValue,0) * _unit - wValue; }
        }

        /// <summary>
        /// 料重换算成肉重
        /// </summary>
        public double weight { get; set; }

        static XmlNode GetFeedDefXml()
        {
            XmlDocument xlmDoc = new XmlDocument();
            xlmDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + "App_Data/FeedDef.xml");
            return xlmDoc.SelectSingleNode("feeddef");
        }

        static List<Feed> GroupByName(List<Feed> feeds)
        {
            var x = from xx in feeds
                     group xx by xx.name into g
                     select new Feed
                     {
                         name = g.Key,
                         wValue = g.Sum(p => p.wValue),
                         nValue = g.Sum(p => p.nValue),
                         weight = g.Sum(p => p.weight),
                     };

            return x.ToList();
        }

        static public List<Feed> GetFeeds(XmlNode feedDefXml, int from, int to,int num)
        {
            List<Feed> feeds = new List<Feed>();
            XmlNode xn = feedDefXml;
            XmlNodeList xnl = xn.ChildNodes;

            foreach (var xnf in xnl)
            {
                XmlElement xe = (XmlElement)xnf;
                int f = Int32.Parse(xe.GetAttribute("from"));
                int t = Int32.Parse(xe.GetAttribute("to"));
                float k = float.Parse(xe.GetAttribute("ratio"));

                if (from > t || to < f)
                    continue;

                XmlNodeList xnlc = xe.ChildNodes;
                int v = f;
                float fv = 0.0f;
                foreach (XmlNode xnfc in xnlc)
                {
                    if (v >= from && v <= to)
                    {
                        fv += float.Parse(xnfc.InnerText);
                    }
                    v++;
                }

                Feed feed = new Feed();
                feed.wValue = fv*num;
                feed.weight = fv / k;
                feed.name = xe.GetAttribute("name");

                feeds.Add(feed);
            }

            return GroupByName(feeds);
        }

        static public List<Feed> GetFeeds(int from, int to, int num)
        {
            var x = GetFeedDefXml();
            return GetFeeds(x, from, to, num);
        }

        /// <summary>
        /// 将饲料列表标准化为字符串,供数据库存储
        /// </summary>
        /// <param name="feeds"></param>
        /// <returns></returns>
        static public string GetRegexStr(List<Feed> feeds)
        {
            StringBuilder s = new StringBuilder("");
            foreach (var f in feeds)
            {
                s.AppendFormat("[{0},{1:f},{2:f},{3:f}]", f.name, f.nValue, f.wValue, f.weight);
            }
            
            return s.ToString();
        }

        /// <summary>
        /// 从标准化的字符串中获取饲料列表
        /// </summary>
        /// <param name="RegexStr"></param>
        /// <returns></returns>
        static public List<Feed> GetFeeds(string RegexStr)
        {
            Regex regex;
            //格式为:[name,nValue,wValue,weight]
            regex = new Regex(@"\[(?<name>\w+),(?<nValue>\d+),(?<wValue>\d+\.?\d*),(?<weight>\d+\.?\d*)\]"
                , RegexOptions.Singleline);
            var matchs = regex.Matches(RegexStr);

            List<Feed> lf = new List<Feed>();
            foreach (Match m in matchs)
            {
                Feed f = new Feed()
                {
                    name = m.Groups["name"].Value,
                    nValue = int.Parse(m.Groups["nValue"].Value),
                    wValue = double.Parse(m.Groups["wValue"].Value),
                    weight = double.Parse(m.Groups["weight"].Value),
                };
                lf.Add(f);
            }
            return lf;
        }


        
    }
}
