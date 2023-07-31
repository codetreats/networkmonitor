using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace ServerMonitor
{
    public class MailBuilder
    {
        private static readonly int sendHour = 18;
        private static readonly int sendMinute = 30;

        private Guardian[] guards;

        private int okCounter = 0;
        private int informationCounter = 0;
        private int warningCounter = 0;
        private int errorCounter = 0;
        private int criticalCounter = 0;
        private int unknownCounter = 0;

        private StringBuilder information, warning, error, critical, unknown;

        public MailBuilder(Guardian[] guards)
        {
            this.guards = guards;

            information = new StringBuilder();
            warning = new StringBuilder();
            error = new StringBuilder();
            critical = new StringBuilder();

            unknown = new StringBuilder();

        }

        public void build(string mailDir)
        {
            int total = 0;
            int evil = 0;
            foreach (Guardian guard in guards)
            {
                total++;
                string line = "- " + guard.getName() + ": " + guard.getText();
                switch (guard.getLevel().getLevel())
                {
                    case 0:
                        okCounter++;
                        break;
                    case 1:
                        informationCounter++;
                        information.AppendLine(line);
                        break;
                    case 2:
                        warningCounter++;
                        evil++;
                        warning.AppendLine(line);
                        break;
                    case 3:
                        errorCounter++;
                        evil++;
                        error.AppendLine(line);
                        break;
                    case 5:
                        criticalCounter++;
                        evil++;
                        critical.AppendLine(line);
                        break;
                    default:
                        unknownCounter++;
                        evil++;
                        unknown.AppendLine(line);
                        break;
                }
            }

            string prefix;
            if (evil == 0)
            {
                prefix = "Alles gut: ";
            }
            else
            {
                prefix = "Achtung: ";
            }

            string subject = prefix + evil + " Problem(e)";
            //subject = prefix + total + " Services, davon " + criticalCounter + " Critical, " + errorCounter + " Error, " + warningCounter + " Warning, " + informationCounter + " Information, " + unknownCounter + " Unbekannt";
            StringBuilder message = new StringBuilder();
            message.AppendLine(okCounter + "/" + total + " OK");
            message.AppendLine(criticalCounter + "/" + total + " Critical");
            message.AppendLine(errorCounter + "/" + total + " Error");
            message.AppendLine(warningCounter + "/" + total + " Warning");
            message.AppendLine(informationCounter + "/" + total + " Information");
            message.AppendLine(unknownCounter + "/" + total + " Unbekannt");
            string summary = message.ToString();
            message.AppendLine();
            if (criticalCounter > 0)
            {
                message.AppendLine("###################### Critical ####################");
                message.AppendLine(critical.ToString());
            }
            if (errorCounter > 0)
            {
                message.AppendLine("###################### Error ####################");
                message.AppendLine(error.ToString());
            }
            if (warningCounter > 0)
            {
                message.AppendLine("###################### Warning ####################");
                message.AppendLine(warning.ToString());
            }
            if (informationCounter > 0)
            {
                message.AppendLine("###################### Information ####################");
                message.AppendLine(information.ToString());
            }
            if (unknownCounter > 0)
            {
                message.AppendLine("###################### Unknown ####################");
                message.AppendLine(unknown.ToString());
            }

            string subjectFilename = Path.Combine(mailDir, "subject.txt");
            string messageFilename = Path.Combine(mailDir, "message.txt");
            string summaryFilename = Path.Combine(mailDir, "sum.txt");

            // Calc summarySum
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(summary);
            byte[] hash = md5.ComputeHash(inputBytes);
            string summarySum = Convert.ToBase64String(hash);

            string oldSum = string.Empty;
            if (File.Exists(summaryFilename))
            {
                oldSum = File.ReadAllText(summaryFilename).Trim();
            }

            File.WriteAllText(summaryFilename, summarySum);
            // Prüfsumme ungleich oder Sendezeitpunkt?
            if (summarySum != oldSum || (DateTime.Now.Hour == sendHour && DateTime.Now.Minute == sendMinute))
            {
                // Senden
                File.WriteAllText(subjectFilename, subject);
                File.WriteAllText(messageFilename, message.ToString());
            }
            else
            {
                // Nicht senden
                File.Delete(subjectFilename);
                File.Delete(messageFilename);
            }

            Log.d(this.GetType().Name, subject);
            Log.d(this.GetType().Name, "\n" + message);
        }
    }
}
