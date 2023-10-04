using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ServerMonitor
{
    public class HtmlBuilder
    {
        private static readonly string root = "index";
        private bool isRoot;

        private string parent;

        private string name;
        private Guardian[] guards;

        private Dictionary<string, bool> parsed;
        private List<Guardian> placeHere;

        private int okCounter = 0;
        private int informationCounter = 0;
        private int warningCounter = 0;
        private int errorCounter = 0;
        private int criticalCounter = 0;
        private int unknownCounter = 0;

        private int evil = 0;
        private int total = 0;

        private StringBuilder information, warning, error, critical, unknown;

        private List<HtmlBuilder> builder;

        private StringBuilder html;

        private int maxCatLevel = -1;
        private int maxGuardLevel = -1;

        public HtmlBuilder(Guardian[] guards)
            : this(root, root, guards, true)
        {
        }

        public HtmlBuilder(string name, string parent, Guardian[] guards)
            : this(name, parent, guards, false)
        {
        }

        private HtmlBuilder(string name, string parent, Guardian[] guards, bool isRoot)
        {
            this.name = name;
            this.parent = parent;
            this.guards = guards;
            this.isRoot = isRoot;

            information = new StringBuilder();
            warning = new StringBuilder();
            error = new StringBuilder();
            critical = new StringBuilder();

            unknown = new StringBuilder();

            html = new StringBuilder();

            parsed = new Dictionary<string, bool>();
            builder = new List<HtmlBuilder>();
            placeHere = new List<Guardian>();
        }

        public void build(string htmlDir)
        {
            Log.d(GetType().Name, "Build: " + name);
            Log.d(GetType().Name, "IsRoot: " + isRoot);

            foreach (Guardian guard in guards)
            {
                string guardName = guard.getName().ToLower();
                if (guardName == name)
                {
                    continue;
                }

                // Ignoriere guards, die nicht in diese Kategorie gehören
                if (!isRoot && !guardName.StartsWith(name.ToLower()))
                {
                    Log.d(GetType().Name, "Ignore " + guardName);
                    continue;
                }

                // Noch mehr Unterkategorien?
                string guardNameWithoutPrefix = guardName;
                if (!isRoot)
                {
                    guardNameWithoutPrefix = guardName.Substring(name.Length + 1);
                }
                Log.d(GetType().Name, "NameWithout:" + guardNameWithoutPrefix);
                string[] parts = guardNameWithoutPrefix.Split('.');

                // Unterkategorie
                if (parts.Length >= 2)
                {
                    string tmp = name + "." + parts[0];
                    if (isRoot)
                    {
                        tmp = parts[0];
                    }
                    if (!parsed.ContainsKey(tmp))
                    {
                        parsed.Add(tmp, true);
                    }
                }

                // Keine Unterkategorie
                if (parts.Length == 1)
                {
                    placeHere.Add(guard);
                }

                makeStat(guard.getLevel().getLevel(), parts.Length <= 1);
            }

            foreach (String nextName in parsed.Keys)
            {
                HtmlBuilder htmlBuilder = new HtmlBuilder(nextName, name, guards);
                htmlBuilder.build(htmlDir);
                builder.Add(htmlBuilder);
            }
            writeHtml(htmlDir);
        }

        private void writeHtml(string htmlDir)
        {
            string title = "Netzwerk-Monitor - " + name;
            if (isRoot)
            {
                title = "Netzwerk-Monitor";
            }

            append("<!doctype html>");
            append("<html>");
            append("  <head>");
            append("    <title>" + title + "</title>");
            append("    <link rel='stylesheet' type='text/css' href='style.css'>");
            append("    <meta charset=\"utf-8\">");

            // Turn of caching
            append("    <meta http-equiv='cache-control' content='max-age=0' />");
            append("    <meta http-equiv='cache-control' content='no-cache' />");
            append("    <meta http-equiv='cache-control' content='no-store' />");
            append("    <meta http-equiv='expires' content='0' />");
            append("    <meta http-equiv='expires' content='Tue, 01 Jan 1980 1:00:00 GMT' />");
            append("    <meta http-equiv='pragma' content='no-cache' />");
            
            if (isRoot)
            {
                append("    <meta http-equiv='refresh' content='60' />");
            }
            else
            {
                append("    <meta http-equiv='refresh' content='60; url=index.html' />");
            }
            append("  </head>");
            append("  <body style='background-image:url(bg.jpg)'>");
            if (!isRoot)
            {
                append("<a href='index.html'><img src='home.png' alt='Home'></a>", 0);
                append("<a href='" + parent + ".html'><img src='back.png' alt='Zurueck'></a>", 0);
            } else {
                append("<a href='pipeline/index.html'><img src='home.png' alt='Pipeline'></a>", 0);
            }
            
            append("<a href='" + name + ".html'><h1>" + title + "</h1></a>", 0);
            append("<h3>" + DateTimePrinter.print() + "</h3>", 0);

            writeStats(this);
            




            // Unterkategorien
            if (builder.Count > 0)
            {
                append("<div class='outer olevel" + maxCatLevel + "'>");
                append("  <h1>Kategorien</h1>");
                append("  <div class='inner'>");
                
                foreach (HtmlBuilder sub in builder)
                {
                    append("    <a href='" + sub.getName() + ".html'>");
                    append("      <div class='service level" + sub.getLevel().getLevel() + "'>");
                    append("          <h1><nobr>" + sub.getName().Replace(".", "</nobr>.<wbr>&#8203;</wbr><nobr>") + "</nobr></h1>");
                    writeStats(sub);
                    append("      </div>");
                    append("    </a>");
                }
                if (isRoot)
                {
                    append("    <a href='log.html'>");
                    append("      <div class='service level0'>");
                    append("          <h1><nobr>log</nobr></h1>");
                    append("          Übersicht der letzten Ereignisse");
                    append("      </div>");
                    append("    </a>");
                }
                append("  </div>");
                append("</div>");  
            }

            append("", 2);

            // Services
            if (placeHere.Count > 0)
            {
                append("<div class='outer olevel" + maxGuardLevel + "'>");
                append("  <h1>Services</h1>");
                append("  <div class='inner'>");

                foreach (Guardian guard in placeHere)
                {
                    append("    <div class='service level" + guard.getLevel().getLevel() + "'>");
                    append("          <h1><nobr>" + guard.getName().Replace(".", "</nobr>.<wbr>&#8203;</wbr><nobr>") + "</nobr></h1>");
                    writeGuard(guard);
                    append("    </div>");
                }
                append("  </div>");
                append("</div>");  
            }
            append("</html>");
            Log.d(GetType().Name, "PWD: " + Directory.GetCurrentDirectory());
            File.WriteAllText(Path.Combine(htmlDir, name + ".html"), html.ToString());
        }

        private void writeStats(HtmlBuilder sub)
        {
            int okCounter = sub.getOKCounter();
            int criticalCounter = sub.getCriticalCounter();
            int errorCounter = sub.getErrorCounter();
            int warningCounter = sub.getWarningCounter();
            int informationCounter = sub.getInformationCounter();
            int unknownCounter = sub.getUnknownCounter();
            int total = sub.getTotalCounter();
            int evil = sub.getEvilCounter();

      
            append( total + " Services: ", 0);
            string delim = "";
            if (evil == 0)
            {
                append("Alles gut!", 0);
            }
            if (criticalCounter > 0)
            {
                append(delim + criticalCounter + " Critical", 0);
                delim = " - ";
            }
            if (errorCounter > 0)
            {
                append(delim + errorCounter + " Error", 0);
                delim = " - ";
            }
            if (warningCounter > 0)
            {
                append(delim + warningCounter + " Warning", 0);
                delim = " - ";
            }
            if (informationCounter > 0)
            {
                append(delim + informationCounter + " Information", 0);
                delim = " - ";
            }
            if (unknownCounter > 0)
            {
                append(delim + unknownCounter + " Information", 0);
                delim = " - ";
            }
        }

        private void writeGuard(Guardian guard)
        {
            append(DateTimePrinter.print(guard.getDateTime()),1);
            append(guard.getText().Replace("\n","</br>"),1);
        }

        private void append(string text)
        {
            append(text, 0);
        }

        private void append(string text, int br)
        {
            string brs = "";
            for (int i = 0; i < br; i++)
            {
                brs += "</br>";
            }
            html.AppendLine(text + brs);
        }

        private void makeStat(int level, bool placeHere)
        {
            if (placeHere && level > maxGuardLevel)
            {
                    maxGuardLevel = level;
             
            }
            if (!placeHere && level > maxCatLevel)
            {
                maxCatLevel = level;
            }
            
            total++;
            switch (level)
            {
                case 0:
                    okCounter++;
                    break;
                case 1:
                    informationCounter++;
                    break;
                case 2:
                    warningCounter++;
                    evil++;
                    break;
                case 3:
                    errorCounter++;
                    evil++;
                    break;
                case 5:
                    criticalCounter++;
                    evil++;
                    break;
                default:
                    unknownCounter++;
                    evil++;
                    break;
            }
        }

        public Level getLevel()
        {
            if (maxCatLevel > maxGuardLevel)
            {
                return Level.parse(maxCatLevel.ToString());
            }
            return Level.parse(maxGuardLevel.ToString());
            
        }

        public string getName()
        {
            return name;
        }

        public int getCriticalCounter()
        {
            return criticalCounter;
        }
        public int getErrorCounter()
        {
            return errorCounter;
        }
        public int getWarningCounter()
        {
            return warningCounter;
        }
        public int getInformationCounter()
        {
            return informationCounter;
        }
        public int getOKCounter()
        {
            return okCounter;
        }
        public int getUnknownCounter()
        {
            return unknownCounter;
        }
        public int getTotalCounter()
        {
            return total;
        }
        public int getEvilCounter()
        {
            return evil;
        }
    }
}
