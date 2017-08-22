using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Farm.Raisers.Feeds
{
    public static class FeedHelper
    {

        
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
            StringBuilder s = new StringBuilder();
            foreach (var f in feeds)
            {
                s.AppendFormat("[{0},{1:f0},{2:f},{3:f}]", f.name, f.nValue, f.wValue, f.weight);
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
            regex = new Regex(@"\[(?<name>\w+),(?<nValue>\d+\.?\d*),(?<wValue>\d+\.?\d*),(?<weight>\d+\.?\d*)\]"
                , RegexOptions.Singleline);
            var matchs = regex.Matches(RegexStr);

            List<Feed> lf = new List<Feed>();
            foreach (Match m in matchs)
            {
                Feed f = new Feed()
                {
                    name = m.Groups["name"].Value,
                    nValue = double.Parse(m.Groups["nValue"].Value),
                    wValue = double.Parse(m.Groups["wValue"].Value),
                    weight = double.Parse(m.Groups["weight"].Value),
                };
                lf.Add(f);
            }
            return lf;
        }

        /// <summary>
        /// 从标准化字符串列表中获取饲料列表，并按饲料名分组
        /// </summary>
        /// <param name="RegexStrs"></param>
        /// <returns></returns>
        static public List<Feed> GetFeeds(List<string> RegexStrs)
        {
            List<Feed> fs = new List<Feed>();
            foreach (var s in RegexStrs)
            {
                fs.AddRange(GetFeeds(s));
            }

            return GroupByName(fs);
        }

        /// <summary>
        /// 将饲料列表转换为字符串
        /// </summary>
        /// <param name="feeds"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        static public string GetFeedsDetail(List<Feed> feeds,int flag=0)
        {
            StringBuilder s = new StringBuilder();
            foreach (var f in feeds)
            {
                switch(flag)
                {
                    case 0:
                        s.AppendFormat("{0}:{1:f0}; ", f.name, f.nValue);
                        break;
                    case 1:
                        s.AppendFormat("{0}:{1:f}/{2:f}; ", f.name,f.wValue, f.nValue);
                        break;
                    case 2:
                        s.AppendFormat("[{0},{1:f0},{2:f},{3:f}]", f.name, f.nValue, f.wValue, f.weight);
                        break;
                    default:
                        s.AppendFormat("[{0},{1:f},{2:f},{3:f}]", f.name, f.nValue, f.wValue, f.weight);
                        break;
                }
            }
            return s.ToString();
        }

        /// <summary>
        /// 标准日龄的饲料转换成肉重
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        static public double NormalWeight(int from,int to)
        {
            List<Feed> feeds = GetFeeds(from, to, 1);
            if (feeds.Count == 0)
                return 0.0;

            return feeds.Sum(p => p.weight);
        }
        
    }
}
