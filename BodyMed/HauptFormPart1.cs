// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HauptFormPart1.Cs" company="Brenners Videotechnik">
//   Copyright (c) Armin Brenner. All rights reserved.
// </copyright>
// <summary>
//   Zusammenfassung f�r HauptForm, ausgelagerte Methoden.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
// <remarks>
//     <para>Autor: Armin Brenner</para>
//     <para>
//        History : Datum     bearb.  �nderung
//                  --------  ------  ------------------------------------
//                  07.06.16  br      Grundversion
//      </para>
// </remarks>
// --------------------------------------------------------------------------------------------------------------------
// ReSharper disable CatchAllClause
namespace BodyMed
{
    using System;
    using System.Data;
    using System.Data.OleDb;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    using Infragistics.Win.UltraWinGrid;

    using static HauptForm.Formular;

    using Resources = Properties.Resources;

    /// <summary>
    /// Zusammenfassung f�r HauptForm.
    /// </summary>
    [SuppressMessage("ReSharper", "EmptyGeneralCatchClause")]
    public partial class HauptForm
    {
        /// <summary>
        /// Behandelt das 'AfterExitEditMode' Ereignis der Grids.
        /// Der Editiermodus im Grid wurde beendet
        /// </summary>
        private void AfterExitEditMode(ref UltraGrid grid, string tabelle)
        {
            this.GetRowIndex(); // Index der aktiven Zeile ermitteln

            // Pr�fen, ob eine Zeile Aktiv ist
            if (grid.ActiveRow == null)
            {
                return; // Abbruch, da keine aktive Zeile
            }

            // Alle Zellen durchgehen, damit ge�nderte Zelle gefunden wird
            for (var colIndex = 0; colIndex < grid.ActiveRow.Cells.Count; colIndex++)
            {
                // Aktive Zelle gefunden -> �berpr�fen, ob Daten ge�ndert wurden
                if (!grid.ActiveRow.Cells[colIndex].DataChanged)
                {
                    // Daten wurden nicht ge�ndert, also n�chste Spalte ausw�hlen
                    continue;
                }

                var aktuellerWert = grid.ActiveRow.Cells[colIndex].Value; // ge�nderter Wert im Grid

                // Nachschauen, ob ein zus�tzlicher Editor f�r die Dateneingabe verwendet wird
                // Wenn ja, Wert aus diesem Editor eintragen
                var editor = grid.ActiveRow.Cells[colIndex].EditorResolved;
                if (editor == null)
                {
                    continue;
                }
                try
                {
                    if (editor.Value != null)
                    {
                        aktuellerWert = editor.Value; // Wert des Editors eintragen
                    }
                }
                catch
                {
                    // ignoriert
                }

                // Spaltennamen und Datentyp der ge�nderten Zelle bestimmen
                // Formular ermitteln
                var spaltenName = string.Empty; // Name der Spalte
                var spaltenTyp = typeof(DBNull); // Daten-Typ der Spalte
                string table = $"{tabelle}"; // zum Zusammensetzen des Tabellen-Namens

                // �berpr�fen,Tabelle angew�hlt ist
                switch (this.selectedTab)
                {
                    case (int)Ernaehrung:
                        {
                            // Es ist die Ern�hrungstabelle angew�hlt
                            spaltenName = string.Format(
                                this.dataSetGewicht1.Tables["{0}"].Columns[colIndex].ToString(),
                                tabelle);
                            spaltenTyp = this.dataSetGewicht1.Tables[table].Rows[this.rowIndex][colIndex].GetType();
                            break;
                        }

                    case (int)BlutDruck:
                        {
                            // Es ist Blutdrucktabelle angew�hlt
                            spaltenName =
                                string.Format(
                                    this.dataSetBlutDruck1.Tables["{0}"].Columns[colIndex].ToString(),
                                    tabelle);
                            spaltenTyp = this.dataSetBlutDruck1.Tables[table].Rows[this.rowIndex][colIndex].GetType();
                            break;
                        }
                }

                // Abfrage der Datentypen
                string strUpdate; // Variable f�r Update-Kommando
                if (spaltenTyp == typeof(string))
                {
                    // Zeichenkette
                    strUpdate =
                        $"Update {tabelle} SET  [{spaltenName}]='{aktuellerWert}' WHERE ([Index]={this.indexNummerAktiveZeile})";
                }
                else if (spaltenTyp == typeof(decimal))
                {
                    // Dezimalzahl
                    strUpdate = string.Format(
                        "Update {3} SET  [{0}]='{1}' WHERE ([Index]={2})",
                        spaltenName,
                        Convert.ToDecimal(aktuellerWert),
                        this.indexNummerAktiveZeile,
                        tabelle);
                }
                else if (spaltenTyp == typeof(ulong))
                {
                    // DWORD
                    strUpdate = string.Format(
                        "Update {3} SET  [{0}]='{1}' WHERE ([Index]={2})",
                        spaltenName,
                        Convert.ToUInt64(aktuellerWert),
                        this.indexNummerAktiveZeile,
                        tabelle);
                }
                else if (spaltenTyp == typeof(DateTime))
                {
                    // Datum
                    strUpdate = string.Format(
                        "Update {3} SET  [{0}]='{1}' WHERE ([Index]={2})",
                        spaltenName,
                        Convert.ToDateTime(aktuellerWert),
                        this.indexNummerAktiveZeile,
                        tabelle);
                }
                else if (spaltenTyp == typeof(bool))
                {
                    // Ja/Nein
                    strUpdate = string.Format(
                        "Update {3} SET  [{0}]='{1}' WHERE ([Index]={2})",
                        spaltenName,
                        aktuellerWert,
                        this.indexNummerAktiveZeile,
                        tabelle);
                }
                else if (spaltenTyp == typeof(short))
                {
                    // 16-Bit Integer
                    strUpdate = string.Format(
                        "Update {3} SET  [{0}]='{1}' WHERE ([Index]={2})",
                        spaltenName,
                        Convert.ToInt16(aktuellerWert),
                        this.indexNummerAktiveZeile,
                        tabelle);
                }
                else if (spaltenTyp == typeof(byte))
                {
                    // Byte
                    strUpdate = string.Format(
                        "Update {3} SET  [{0}]='{1}' WHERE ([Index]={2})",
                        spaltenName,
                        Convert.ToByte(aktuellerWert),
                        this.indexNummerAktiveZeile,
                        tabelle);
                }
                else if (spaltenTyp == typeof(DBNull))
                {
                    // Nicht vorhandener Wert
                    strUpdate = string.Format(
                        "Update {3} SET  [{0}]='{1}' WHERE ([Index]={2})",
                        spaltenName,
                        aktuellerWert,
                        this.indexNummerAktiveZeile,
                        tabelle);
                }
                else
                {
                    // 32-Bit Integer
                    strUpdate = string.Format(
                        "Update {3} SET  [{0}]='{1}' WHERE ([Index]={2})",
                        spaltenName,
                        Convert.ToInt32(aktuellerWert),
                        this.indexNummerAktiveZeile,
                        tabelle);
                }

                this.ExecuteQuery(strUpdate);                                   // Update in Datenbank durchf�hren
            }
        }

        /// <summary>
        /// Abfrage auf Datenbank ausf�hren.
        /// Die Verbindung zur Datenbank steht in der Variablen <see cref="oleDbConnection1" />
        /// </summary>
        /// <param name="strQuery">Der  Abfrageausdruck.</param>
        /// <returns>Anzahl der betroffenen Zeilen, bei einem Fehler -1</returns>
        /// <exception cref="Exception">Wenn Abfrage fehlgeschlagen ist</exception>
        // ReSharper disable once UnusedMethodReturnValue.Local
        private int ExecuteQuery(string strQuery)
        {
            var ret = 0;                                                        // Abfrage ist fehlgeschlagen vorgeben
            try
            {
                this.oleDbCommand1 = new OleDbCommand(strQuery, this.oleDbConnection1) { CommandType = CommandType.Text }; // Neue Instanz des Abfragekommandos erzeugen
                ret = this.oleDbCommand1.ExecuteNonQuery();                     // Abfrage durchf�hren
            }
            catch (Exception ex)
            {
                // Falls schief gelaufen, Grund anzeigen
                MessageBox.Show(ex.ToString(), Resources.HauptForm_ExecuteQuery_Fehler_beim_Speichern_der_Daten, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

           return ret;                                                          // Ergebnis zur�ckgeben                                          
        }
        #region Drucken

        /// <summary>
        /// Wird ausgef�hrt, wenn das Druck-Kommando ausgef�hrt werden soll.
        /// Ausgew�hltes Grid ausdrucken
        /// </summary>
        private void PintExecuted()
        {
            // �berpr�fen, welches Werkzeug angew�hlt ist
            //switch (WerkzeugArt)
            //{
            //    case (ushort)Werkzeug.FlexToolE05:
            //        {
            //            // Werkzeug ist ein FlexTool f�r E05 -> Ausw�hlen, ob Geberdaten oder Motordaten gedruckt werden soll
            //            this.ultraGridPrintDocument1.Grid =
            //                this.geberEingabe ? this.ultraGridGeber : this.ultraGridMotor;
            //            break;
            //        }

            //    case (ushort)Werkzeug.FuegeModul:
            //        {
            //            ultraGridPrintDocument1.Grid = ultraGridFuegemodul;     // F�gemoduldaten sollen gedruckt werden
            //            break;
            //        }

            //    case (ushort)Werkzeug.FlexToolE12:
            //        {
            //            // Werkzeug ist ein FlexTool f�r E12 -> Ausw�hlen, ob Geberdaten oder Motordaten gedruckt werden soll
            //            this.ultraGridPrintDocument1.Grid =
            //                this.geberEingabe ? this.ultraGridGeberE12 : this.ultraGridMotorE12;
            //            break;
            //        }
            //}

            //// Druckdialog anzeigen
            //this.printDialog1.Document = this.ultraGridPrintDocument1;          // auszudruckendes Dokument an den Druckdialog �bergeben

            //// Abfragen, ob Ausdruck gestartet werden soll
            //if (this.printDialog1.ShowDialog() != DialogResult.OK)
            //{
            //    return;                                                         // Abbruch Drucken
            //}

            //this.ultraGridPrintDocument1.Print();                               // Aktives Grid ausdrucken
        }

        /// <summary>
        /// Wird ausgef�hrt, wenn das Druckvorschau-Kommando ausgef�hrt werden soll.
        /// Druckvorschau f�r ausgew�hltes Grid
        /// </summary>
        private void PrintPreviewExecuted()
        {
            //// �berpr�fen, welches Werkzeug angew�hlt ist
            //switch (WerkzeugArt)
            //{
            //    case (ushort)Werkzeug.FlexToolE05:
            //        {
            //            // Werkzeug ist ein FlexTool E05-Werkzeug -> �berpr�fen, ob Geber oder Motor
            //            if (geberEingabe)
            //            {
            //                // Druckvorschau f�r Geberdaten
            //                this.ultraGridPrintDocument1.Grid = this.ultraGridGeber;
            //                this.ultraPrintPreviewDialog1.Document.DocumentName = "Geberchips E05";
            //                this.ultraPrintPreviewDialog1.Text = "Druckvorschau Geberchips E05";
            //                this.ultraPrintPreviewDialog1.ShowDialog(this);
            //            }
            //            else
            //            {
            //                // Druckvorschau f�r Motordaten
            //                this.ultraGridPrintDocument1.Grid = ultraGridMotor;
            //                this.ultraPrintPreviewDialog1.Text = "Druckvorschau Motorchips E05";
            //                this.ultraPrintPreviewDialog1.Document.DocumentName = "Motorchips E05";
            //                this.ultraPrintPreviewDialog1.ShowDialog(this);
            //            }

            //            break;
            //        }

            //    case (ushort)Werkzeug.FuegeModul:
            //        {
            //            // Werkzeug ist ein F�gemodul -> Druckvorschau f�r F�gemoduldaten
            //            this.ultraGridPrintDocument1.Grid = ultraGridFuegemodul;
            //            this.ultraPrintPreviewDialog1.Text = "Druckvorschau F�gemodul";
            //            this.ultraPrintPreviewDialog1.Document.DocumentName = "F�gemodul";
            //            this.ultraPrintPreviewDialog1.ShowDialog(this);
            //            break;
            //        }

            //    case (ushort)Werkzeug.FlexToolE12:
            //        {
            //            // Werkzeug ist ein FlexTool E12-Werkzeug -> �berpr�fen, ob Geber oder Motor
            //            if (geberEingabe)
            //            {
            //                // Druckvorschau f�r Geberdaten
            //                this.ultraGridPrintDocument1.Grid = ultraGridGeberE12;
            //                this.ultraPrintPreviewDialog1.Document.DocumentName = "Geberchips E12";
            //                this.ultraPrintPreviewDialog1.Text = "Druckvorschau Geberchips E12";
            //                this.ultraPrintPreviewDialog1.ShowDialog(this);
            //            }
            //            else
            //            {
            //                // Druckvorschau f�r Motordaten
            //                this.ultraGridPrintDocument1.Grid = ultraGridMotorE12;
            //                this.ultraPrintPreviewDialog1.Text = "Druckvorschau Motorchips E12";
            //                this.ultraPrintPreviewDialog1.Document.DocumentName = "Motorchips E12";
            //                this.ultraPrintPreviewDialog1.ShowDialog(this);
            //            }

            //            break;
            //        }
            //}
        }

        /// <summary>
        /// Drucker einstellen.
        /// </summary>
        /// <param name="e">Die <see cref="Infragistics.Win.UltraWinGrid.CancelablePrintEventArgs"/> Instanz, welche die Ereignisdaten enth�lt.</param>
        private void SetupPrint(CancelablePrintEventArgs e)
        {
            //// Es sollen alle Spalten gedruckt werden, auch die versteckten
            //int spalte;                                                         // Z�hlvariable

            //switch (WerkzeugArt)
            //{
            //    case (ushort)Werkzeug.FlexToolE05:
            //        {
            //            // Es ist ein flexToolE05-Werkzeug ausgew�hlt -> �berpr�fen, welche Tabelle bearbeitet wird
            //            if (geberEingabe)
            //            {
            //                // Seiten-Layout f�r Geberdaten
            //                e.DefaultLogicalPageLayoutInfo.PageHeader = "Geberchips f�r E05";
            //                for (spalte = 1; spalte < this.ultraGridGeber.DisplayLayout.Bands[0].Columns.Count; spalte++)
            //                {
            //                    this.ultraGridGeber.DisplayLayout.Bands[0].Columns[spalte].Hidden = false; // Es gibt keine versteckten Spalten
            //                }
            //            }
            //            else
            //            {
            //                // Seiten-Layout f�r Motordaten
            //                e.DefaultLogicalPageLayoutInfo.PageHeader = "Motorchips f�r E05";
            //                for (spalte = 1; spalte < this.ultraGridMotor.DisplayLayout.Bands[0].Columns.Count; spalte++)
            //                {
            //                    this.ultraGridMotor.DisplayLayout.Bands[0].Columns[spalte].Hidden = false; // Es gibt keine versteckten Spalten
            //                }
            //            }

            //            break;
            //        }

            //    case (ushort)Werkzeug.FlexToolE12:
            //        {
            //            // Es ist ein flexToolE12-Werkzeug ausgew�hlt -> �berpr�fen, welche Tabelle bearbeitet wird
            //            if (geberEingabe)
            //            {
            //                // Seiten-Layout f�r Geberdaten
            //                e.DefaultLogicalPageLayoutInfo.PageHeader = "Geberchips f�r E12";
            //                for (spalte = 1; spalte < this.ultraGridGeberE12.DisplayLayout.Bands[0].Columns.Count; spalte++)
            //                {
            //                    this.ultraGridGeberE12.DisplayLayout.Bands[0].Columns[spalte].Hidden = false; // Es gibt keine versteckten Spalten
            //                }
            //            }
            //            else
            //            {
            //                // Seiten-Layout f�r Motordaten
            //                e.DefaultLogicalPageLayoutInfo.PageHeader = "Motorchips f�r E12";
            //                for (spalte = 1; spalte < this.ultraGridMotorE12.DisplayLayout.Bands[0].Columns.Count; spalte++)
            //                {
            //                    this.ultraGridMotorE12.DisplayLayout.Bands[0].Columns[spalte].Hidden = false; // Es gibt keine versteckten Spalten
            //                }
            //            }

            //            break;
            //        }

            //    case (ushort)Werkzeug.FuegeModul:
            //        {
            //            // Seiten-Layout f�r F�gemoduldaten
            //            e.DefaultLogicalPageLayoutInfo.PageHeader = "F�gemodul";
            //            for (spalte = 1; spalte < this.ultraGridFuegemodul.DisplayLayout.Bands[0].Columns.Count; spalte++)
            //            {
            //                this.ultraGridFuegemodul.DisplayLayout.Bands[0].Columns[spalte].Hidden = false; // Es gibt keine versteckten Spalten
            //            }
            //        }

            //        break;
            //}

            // Allgemeine Einstellungen f�r das Seiten-Layout
            e.DefaultLogicalPageLayoutInfo.PageHeaderHeight = 40;               // Kopfh�he
            e.DefaultLogicalPageLayoutInfo.PageHeaderAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True; // Kopf wird fett gedruckt..
            e.DefaultLogicalPageLayoutInfo.PageHeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center; // ..und ist zentriert
            e.DefaultLogicalPageLayoutInfo.PageHeaderAppearance.FontData.SizeInPoints = 20; // Schriftgr��e des Kopfes
            e.PrintDocument.DefaultPageSettings.Landscape = true;               // Querformat einstellen
        }

        /// <summary>
        /// Wird aufgerufen, wenn das Dokument ausgedruckt wurde
        /// </summary>
        private void UltraGridPrintDocumentEndPrint()
        {
            //ultraGridPrintDocument1.Grid.DisplayLayout.Appearance.BackColor2 = Color.FromArgb(233, 242, 199);
            //int tabIndex;                                                       // Ausgew�hltes Tab

            //// Urspr�ngliche Tabulatorauswahl f�r die Anzeige im Grid wieder herstellen, dazu �berpr�fen,
            //// welches Werkzeug angew�hlt ist.
            //switch (WerkzeugArt)
            //{
            //    case (ushort)Werkzeug.FlexToolE05:
            //        {
            //            // Es ist ein flexToolE05-Werkzeug ausgew�hlt
            //            tabIndex = this.ultraTabStripControlFlexTool.SelectedTab.Index;
            //            this.ultraTabStripControlFlexTool.SelectedTab = tabIndex == 0 ? this.ultraTabStripControlFlexTool.Tabs[1] : this.ultraTabStripControlFlexTool.Tabs[0];

            //            Application.DoEvents(); // Nachrichtenbearbeitung durchf�hren
            //            this.ultraTabStripControlFlexTool.SelectedTab = this.ultraTabStripControlFlexTool.Tabs[tabIndex]; // Urspr�ngliche Tabulatorauswahl wieder herstellen
            //            break;
            //        }

            //    case (ushort)Werkzeug.FlexToolE12:
            //        {
            //            // Es ist ein flexToolE12-Werkzeug ausgew�hlt
            //            tabIndex = this.ultraTabStripControlFlexToolE12.SelectedTab.Index;
            //            this.ultraTabStripControlFlexToolE12.SelectedTab = tabIndex == 0 ? this.ultraTabStripControlFlexToolE12.Tabs[1] : this.ultraTabStripControlFlexToolE12.Tabs[0];

            //            Application.DoEvents(); // Nachrichtenbearbeitung durchf�hren
            //            this.ultraTabStripControlFlexToolE12.SelectedTab = this.ultraTabStripControlFlexToolE12.Tabs[tabIndex]; // Urspr�ngliche Tabulatorauswahl wieder herstellen
            //            break;
            //        }

            //    case (ushort)Werkzeug.FuegeModul:
            //        {
            //            // Es ist ein F�gemodul angew�hlt
            //            tabIndex = this.ultraTabStripFuegeModul.SelectedTab.Index;
            //            this.ultraTabStripFuegeModul.SelectedTab = tabIndex == 0 ? this.ultraTabStripControlFlexTool.Tabs[1] : this.ultraTabStripControlFlexTool.Tabs[0];

            //            Application.DoEvents(); // Nachrichtenbearbeitung durchf�hren
            //            this.ultraTabStripFuegeModul.SelectedTab = this.ultraTabStripControlFlexTool.Tabs[tabIndex]; // Urspr�ngliche Tabulatorauswahl wieder herstellen
            //            break;
            //        }
            //}
        }

        /// <summary>
        /// Methode wird vor dem Ausdruck aufgerufen
        /// </summary>
        private void UltraGridPrintDocumentBeginPrint()
        {
            //// Alle Spalten des auszudruckenden Grids sichtbar machen
            //for (var spalte = 1; spalte < this.ultraGridPrintDocument1.Grid.DisplayLayout.Bands[0].Columns.Count; spalte++)
            //{
            //    this.ultraGridPrintDocument1.Grid.DisplayLayout.Bands[0].Columns[spalte].Hidden = false; // Beim Ausdruck gibt es keine versteckten Spalten
            //}

            //// Fu�zeile einstellen (Seiten-Nummer eintragen)
            //this.ultraGridPrintDocument1.Footer.TextCenter = "Seite [Page #]";
            //this.ultraGridPrintDocument1.Grid.DisplayLayout.Appearance.BackColor2 = Color.White; // Wei�e Hintergrundfarbe f�r Ausdruck, um Lesbarkeit zu erh�hen
            //this.ultraGridPrintDocument1.Grid.DisplayLayout.Appearance.ForeColor = Color.Black;  // Schwarze Schriftfarbe f�r Ausdruck, um Lesbarkeit zu erh�hen
        }

        /// <summary>
        /// Initialisierung eines Grids f�r den Ausdruck
        /// </summary>
        /// <param name="e">Die <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> Instanz, welche die Ereignisdaten enth�lt.</param>
        private void UltraGridInitializePrint(CancelablePrintEventArgs e)
        {
            this.SetupPrint(e);                                                 // Ausdruck einstellen
        }

        #endregion Drucken

        /// <summary>
        /// Alle Auswahlen in einem Grid l�schen.
        /// </summary>
        /// <param name="grid">Das zu bearbeitende Grid.</param>
        private void LoescheAuswahl(UltraGridBase grid)
        {
            // alle Zeilen des Grids durchgehen
            foreach (var row in grid.Rows)
            {
                row.Selected = false;                                           // Zeile ist nicht ausgew�hlt
            }

            grid.Invalidate(true);
        }

        /// <summary>
        /// zeile in einem Grid ausw�hlen.
        /// </summary>
        /// <param name="grid">Das zu bearbeitende Grid.</param>
        private void SetzeAuswahl(UltraGridBase grid)
        {
            foreach (var row in grid.Rows.Where(row => Convert.ToInt32(row.Cells[0].Value) == Convert.ToInt32(this.indexNummerAktiveZeile)))
            {
                // Index gefunden, Datenzeile aktivieren und ausw�hlen
                row.Selected = true;
                row.Activate();
                break;
            }

            // Falls keine aktive Zeile vorhanden ist, abbrechen
            if (grid.ActiveRow == null)
            {
                return;                                                         // Abbruch, da keine aktive Zeile
            }

            // Falls eine aktive Zeile existiert, diese anzeigen
            grid.ActiveRowScrollRegion.ScrollRowIntoView(grid.ActiveRow);       // Damit Zeile, auf die gescrollt wird, auch sichtbar ist
        }

        /// <summary>
        /// Datens�tze im DataAdapter auffrischen.
        /// </summary>
        private void RefreshDataSet(ref UltraGrid grid, string tabelle)
        {
            var index = -1;                                                     // Damit bei einem Fehler keine Zeile aktiviert wird

            // Individuelle Bearbeitung je nach ausgew�hlter Ansicht
            if (this.selectedTab == (int)Ernaehrung)
            {
                this.dataSetGewicht1.Tables["Gewicht"].Clear();                                             // Inhalt des Datensatzes f�r Ern�hrungs-Daten l�schen..
               index = this.oleDbDataAdapterGewicht.Fill(this.dataSetGewicht1.Tables["Gewicht"]);           // .. und neu laden
            }
            else
            {
                this.dataSetBlutDruck1.Tables["Gewicht"].Clear();                                           // Inhalt des Datensatzes f�r Ern�hrungs-Daten l�schen..
                index = this.oleDbDataAdapterGewicht.Fill(this.dataSetGewicht1.Tables["BlutdruckDaten"]);   // .. und neu laden
            }

            // aktive Zeile neu bestimmen
            if (index >= 0)
            {
                // Falls die bisher aktive Zeile gel�scht wurde und dies die letzte Zele war,
                // muss die aktive Zeile neu bestimmt werden
                if (index >= grid.Rows.Count)
                {
                    index = grid.Rows.Count - 1;                                // Index auf jetzige letzte Zeile des Grids positionieren
                }

                // Falls Einf�gen durch Fehler abgebrochen wurde, darf keine Zeile aktiviert werden
                if (index < 0)
                {
                    return;
                }

                grid.Refresh();                                                 // Bildschirmanzeige auffrischen
                grid.Rows[index].Activate();                                    // betroffene Zeile aktivieren      
            }

            this.DisplayRecordNumbers();                                        // Anzahl Datens�tze in der Statusleiste anzeigen
        }
    }
}
