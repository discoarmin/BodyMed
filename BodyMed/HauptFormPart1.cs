// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HauptFormPart1.Cs" company="Brenners Videotechnik">
//   Copyright (c) Armin Brenner. All rights reserved.
// </copyright>
// <summary>
//   Zusammenfassung für HauptForm, ausgelagerte Methoden.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
// <remarks>
//     <para>Autor: Armin Brenner</para>
//     <para>
//        History : Datum     bearb.  Änderung
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
    using System.Windows.Forms;

    using Infragistics.Win.UltraWinGrid;

    using static HauptForm.Formular;

    using Resources = Properties.Resources;

    /// <summary>
    /// Zusammenfassung für HauptForm.
    /// </summary>
    [SuppressMessage("ReSharper", "EmptyGeneralCatchClause")]
    public partial class HauptForm
    {
        /// <summary>
        /// Behandelt das 'AfterExitEditMode' Ereignis des ultraGridErnaehrung Controls.
        /// </summary>
        /// Der Editiermodus im ultraGridMotor wurde beendet
        private void AfterExitEditMode(ref UltraGrid grid, string tabelle)
        {
            this.GetRowIndex(); // Index der aktiven Zeile ermitteln

            // Prüfen, ob eine Zeile Aktiv ist
            if (grid.ActiveRow == null)
            {
                return; // Abbruch, da keine aktive Zeile
            }

            // Alle Zellen durchgehen, damit geänderte Zelle gefunden wird
            for (var colIndex = 0; colIndex < grid.ActiveRow.Cells.Count; colIndex++)
            {
                // Aktive Zelle gefunden -> überprüfen, ob Daten geändert wurden
                if (!grid.ActiveRow.Cells[colIndex].DataChanged)
                {
                    // Daten wurden nicht geändert, also nächste Spalte auswählen
                    continue;
                }

                var aktuellerWert = grid.ActiveRow.Cells[colIndex].Value; // geänderter Wert im Grid

                // Nachschauen, ob ein zusätzlicher Editor für die Dateneingabe verwendet wird
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

                // Spaltennamen und Datentyp der geänderten Zelle bestimmen
                // Formular ermitteln
                var spaltenName = string.Empty; // Name der Spalte
                var spaltenTyp = typeof(DBNull); // Daten-Typ der Spalte
                string table = $"{tabelle}"; // zum Zusammensetzen des Tabellen-Namens

                // Überprüfen,Tabelle angewählt ist
                switch (this.selectedTab)
                {
                    case (int)Ernaehrung:
                        {
                            // Es ist die Ernährungstabelle angewählt
                            spaltenName = string.Format(
                                this.dataSetGewicht1.Tables["{0}"].Columns[colIndex].ToString(),
                                tabelle);
                            spaltenTyp = this.dataSetGewicht1.Tables[table].Rows[this.rowIndex][colIndex].GetType();
                            break;
                        }

                    case (int)BlutDruck:
                        {
                            // Es ist Blutdrucktabelle angewählt
                            spaltenName =
                                string.Format(
                                    this.dataSetBlutDruck1.Tables["{0}"].Columns[colIndex].ToString(),
                                    tabelle);
                            spaltenTyp = this.dataSetBlutDruck1.Tables[table].Rows[this.rowIndex][colIndex].GetType();
                            break;
                        }
                }

                // Abfrage der Datentypen
                string strUpdate; // Variable für Update-Kommando
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

                this.ExecuteQuery(strUpdate);                                   // Update in Datenbank durchführen
            }
        }

        /// <summary>
        /// Abfrage auf Datenbank ausführen.
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
                ret = this.oleDbCommand1.ExecuteNonQuery();                     // Abfrage durchführen
            }
            catch (Exception ex)
            {
                // Falls schief gelaufen, Grund anzeigen
                MessageBox.Show(ex.ToString(), Resources.HauptForm_ExecuteQuery_Fehler_beim_Speichern_der_Daten, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

           return ret;                                                          // Ergebnis zurückgeben                                          
        }
    }
}