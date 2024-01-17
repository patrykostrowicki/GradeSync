using System;
using System.Drawing;
using System.Windows.Forms;

namespace GradeSync.klasy
{
    internal class wspólneMetody
    {
        internal static void StylizujDataGridView(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.DefaultCellStyle.SelectionBackColor = Color.DarkCyan;
            dgv.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dgv.BackgroundColor = Color.White;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(48, 63, 159);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10);

            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.GridColor = Color.FromArgb(68, 68, 68);
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 37, 38);
        }

        internal static void Log(string text, RichTextBox rt)
        {
            rt.Invoke(new MethodInvoker(delegate () { rt.AppendText(text + "\r\n"); rt.ScrollToCaret(); }));
        }

        internal static int SprawdzSemestr()
        {
            DateTime dzisiaj = DateTime.Today;
            if (dzisiaj >= new DateTime(dzisiaj.Year, 9, 1) && dzisiaj <= new DateTime(dzisiaj.Year, 12, 15))
            {
                return 1;
            }
            else if ((dzisiaj >= new DateTime(dzisiaj.Year, 12, 16) && dzisiaj.Year == dzisiaj.Year) ||
                     (dzisiaj <= new DateTime(dzisiaj.Year, 6, 21) && dzisiaj.Year == dzisiaj.Year))
            {
                return 2;
            }
            return 0;
        }

    }
}
