using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;



namespace dataFetch
{
    class Program
    {
        static void Main(string[] args)
        {


            int dayloop = -5;
            while (dayloop < 16)
            {


                DateTime dataDate = DateTime.Today.AddDays(dayloop);
                string result;
                try
                {
                    Console.WriteLine(dataDate.ToString("yyyyMMdd"));
                    //Fetch GameList
                    string url = "http://gd2.mlb.com/components/game/mlb/year_" + dataDate.ToString("yyyy") + "/month_" + dataDate.ToString("MM") + "/day_" + dataDate.ToString("dd") + "/master_scoreboard.xml";
                    var request = WebRequest.Create(url);
                    request.ContentType = "application/xml; charset=utf-8";
                    string text;
                    var response = (HttpWebResponse)request.GetResponse();

                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        text = sr.ReadToEnd();
                    }

                    XmlDocument mlbgames = new XmlDocument();
                    mlbgames.LoadXml(text);
                    XmlNodeList games = mlbgames.SelectNodes("/games/game/@game_data_directory");

                    string filepath = "";
                    filepath = "c:\\nbadata\\" + dataDate.ToString("yyyy") + "\\mlb_scoreboard_" + dataDate.ToString("yyyy") + dataDate.ToString("MM") + dataDate.ToString("dd") + ".xml";
                    File.WriteAllText(filepath, text);


                    for (int i = 0; i < games.Count; i++)
                    {
                        try
                        {
                            Console.WriteLine("Fetching Boxscore for " + games[i].InnerText);
                            url = "http://gd2.mlb.com" + games[i].InnerText + "/boxscore.xml";
                            if (dataDate.Year < 2014) { url = "http://gd2.mlb.com/components/game/mlb/year_" + dataDate.ToString("yyyy") + "/month_" + dataDate.ToString("MM") + "/day_" + dataDate.ToString("dd") + "/" + games[i].InnerText.Substring(29) + "/boxscore.xml"; }
                            request = WebRequest.Create(url);
                            request.ContentType = "application/xml; charset=utf-8";
                            Console.WriteLine (url);
                            response = (HttpWebResponse)request.GetResponse();

                            using (var sr = new StreamReader(response.GetResponseStream()))
                            {
                                text = sr.ReadToEnd();
                            }
                            filepath = "c:\\nbadata\\" + dataDate.ToString("yyyy") + "\\mlb_boxscore" + games[i].InnerText.Replace("/", "_") + ".xml";
                            File.WriteAllText(filepath, text);
                        }
                        catch (WebException ex)
                        {
                            result = string.Format("Could not get data. {0}", ex);
                            Console.WriteLine(result);
                        }

                        try
                        {
                            Console.WriteLine("Fetching Game Events " + games[i].InnerText);
                            url = "http://gd2.mlb.com" + games[i].InnerText + "/game_events.xml";
                            if (dataDate.Year < 2014) { url = "http://gd2.mlb.com/components/game/mlb/year_" + dataDate.ToString("yyyy") + "/month_" + dataDate.ToString("MM") + "/day_" + dataDate.ToString("dd") + "/" + games[i].InnerText.Substring(29) + "/game_events.xml"; }
                            request = WebRequest.Create(url);
                            request.ContentType = "application/xml; charset=utf-8";
                            response = (HttpWebResponse)request.GetResponse();

                            using (var sr2 = new StreamReader(response.GetResponseStream()))
                            {
                                text = sr2.ReadToEnd();
                            }
                            filepath = "c:\\nbadata\\" + dataDate.ToString("yyyy") + "\\mlb_game_events" + games[i].InnerText.Replace("/", "_") + ".xml";
                            File.WriteAllText(filepath, text);
                        }
                        catch (WebException ex)
                        {
                            result = string.Format("Could not get data. {0}", ex);
                            Console.WriteLine(result);
                        }

                        try
                        {
                            Console.WriteLine("Fetching Players " + games[i].InnerText);
                            url = "http://gd2.mlb.com" + games[i].InnerText + "/players.xml";
                            if (dataDate.Year < 2014) { url = "http://gd2.mlb.com/components/game/mlb/year_" + dataDate.ToString("yyyy") + "/month_" + dataDate.ToString("MM") + "/day_" + dataDate.ToString("dd") + "/" + games[i].InnerText.Substring(29) + "/players.xml"; }
                            request = WebRequest.Create(url);
                            request.ContentType = "application/xml; charset=utf-8";
                            response = (HttpWebResponse)request.GetResponse();

                            using (var sr2 = new StreamReader(response.GetResponseStream()))
                            {
                                text = sr2.ReadToEnd();
                            }
                            filepath = "c:\\nbadata\\" + dataDate.ToString("yyyy") + "\\mlb_players" + games[i].InnerText.Replace("/", "_") + ".xml";
                            File.WriteAllText(filepath, text);
                        }
                        catch (WebException ex)
                        {
                            result = string.Format("Could not get data. {0}", ex);
                            Console.WriteLine(result);
                        }
                    }
                }
                catch (WebException ex)
                {
                    result = string.Format("Could not get data. {0}", ex);
                    Console.WriteLine( result);
                }

                dayloop++;
            }
         
        }
    }
}
