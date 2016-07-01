// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HauptFormEreignisseErnaehrung.Cs" company="Brenners Videotechnik">
//   Copyright (c) Brenners Videotechnik. All rights reserved.
// </copyright>
// <summary>
//   Zusammenfassung für HauptForm.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
// <remarks>
//     <para>Autor: Armin Brenner</para>
//     <para>
//        History : Datum     bearb.  Änderung
//                  --------  ------  ------------------------------------
//                  14.06.16  br      Grundversion
//      </para>
// </remarks>
// --------------------------------------------------------------------------------------------------------------------
namespace BodyMed
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Forms;

    using static HauptForm.Formular;

    using Resources = Properties.Resources;

    /// <summary>
    /// Ereignisse für Blutdruckdaten.
    /// </summary>
    [SuppressMessage("ReSharper", "RedundantEmptyDefaultSwitchBranch")]
    [SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases")]
    public partial class HauptForm : RibbonForm
    {
        /// <summary>Bearbeitet die Aktivierung einer Zelle .</summary>
        private void UltraGridCellActivated()
        {
            // Individuelle Bearbeitung je nach ausgewählter Ansicht
            if (this.selectedTab == (int)Ernaehrung)
            {
                this.blutDruckEinstellen = false;                               // Keine Blutdruckeingabe
                this.gewichtEinstellen = true;                                  // Gewichtseingabe ist aktiviert
                this.GetRowIndex();                                             // Index der aktiven Zeile ermitteln
            }
            else
            {
                this.blutDruckEinstellen = true;                                // Blutdruckeingabe ist aktiviert
                this.gewichtEinstellen = false;                                 // keine Gewichtseingabe
                this.GetRowIndex();                                             // Index der aktiven Zeile ermitteln
            }
        }

        /// <summary>Bearbeitet die Aktivierung einer Zeile .</summary>
        /// <param name="e">Die <see cref="EventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void UltraGridAfterRowActivate(EventArgs e)
        {
            // Individuelle Bearbeitung je nach ausgewählter Ansicht
            if (this.selectedTab == (int)Ernaehrung)
            {
                // Abbrechen, wenn keine gültigen Daten vorhanden sind
                if (e == EventArgs.Empty || this.bindingManagerGewicht == null)
                {
                    return;                                                     // Abbruch, da keine Ereignisdaten übergeben wurden oder keine Verbindungsdaten zur Datenbank vorhanden sind
                }

                this.blutDruckEinstellen = false;                               // Keine Blutdruckeingabe
                this.gewichtEinstellen = true;                                  // Gewichtseingabe ist aktiviert
                this.GetRowIndex();                                             // Index der aktiven Zeile ermitteln
                this.DisplayRecordNumbers();                                    // Nummer des aktuellen Datensatzes anzeigen ...
                this.rowPosMerkErnaehrung = this.sliderErnaehrung.Value;        // ... und merken
            }
            else
            {
                // Abbrechen, wenn keine gültigen Daten vorhanden sind
                if (e == EventArgs.Empty || this.bindingManagerBlutDruck == null)
                {
                    return;                                                     // Abbruch, da keine Ereignisdaten übergeben wurden oder keine Verbindungsdaten zur Datenbank vorhanden sind
                }

                this.blutDruckEinstellen = true;                                // Blutdruckeingabe ist aktiviert
                this.gewichtEinstellen = false;                                 // keine Gewichtseingabe
                this.GetRowIndex();                                             // Index der aktiven Zeile ermitteln
                this.DisplayRecordNumbers();                                    // Nummer des aktuellen Datensatzes anzeigen ...
                this.rowPosMerkBlutDruck = this.sliderBlutDruck.Value;          // ... und merken
            }
        }

        /// <summary>Bearbeitet das Einfügen einer Zeile .</summary>
        private void UltraGridAfterRowInsert()
        {
            this.auswahllisteLoeschen = true;                                   // Markierte Zeilen eines Grids dürfen neu ermittelt werden
            string strInsert;                                                   // Variable für das Einfügekommando


            // Individuelle Bearbeitung je nach ausgewählter Ansicht
            if (this.selectedTab == (int)Ernaehrung)
            {
                // Es ist die Gewichts-Tabelle angewählt
                // Es muss zuersrt die Grösse ermittelt werden . Die Grösse steht in der Textboc 'tbGroesse'
                var groesse = this.tbGroesse.Text;
                try
                {
                    strInsert = "INSERT INTO[Gewicht] ([Datum], [KG], [FM], [FFM], [KW], [Bmi], [Bemerkung], [Grösse])" 
                        + " VALUES("
                        + "'" + DateTime.Now + "'" + "," + "0.0 , 0.0, 0.0, 0.0, NULL, NULL, "
                        + groesse + ")";                                        // leeren Datensatz einfügen

                    this.oleDbDataAdapterGewicht.InsertCommand.CommandText = strInsert;   // Einfügekommando an DataAdapter übergeben
                    this.oleDbDataAdapterGewicht.InsertCommand.ExecuteNonQuery();         // Einfügen durchführen
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Resources.HauptForm_UltraGridAfterRowInsert_Fehler_beim_Einfügen_eines_neuen_Datensatzes_in_die_Gewichtstabelle__ + ex.Message,
                        Resources.HauptForm_UltraGridAfterRowInsert_Einfügefehler,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                this.RefreshDataSet(ref this.ultraGridErnaehrung, "Gewicht");   // Datensätze im DataAdapter auffrischen
            }
            else
            {
                // Es ist die Blutdruck-Tabelle angewählt
                try
                {
                    strInsert = "INSERT INTO[BlutdruckDaten] ([Datum], [Systolisch], [Diastolisch], [Puls])" + " VALUES("
                            + "'" + DateTime.Now + "'" + "," + "0 , 0, 0)";     // leeren Datensatz einfügen

                    this.oleDbDataAdapterBlutDruck.InsertCommand.CommandText = strInsert; // Einfügekommando an DataAdapter übergeben
                    this.oleDbDataAdapterBlutDruck.InsertCommand.ExecuteNonQuery();       // Einfügen durchführen
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Resources.HauptForm_UltraGridAfterRowInsert_Fehler_beim_Einfügen_eines_neuen_Datensatzes_in_die_Butdruck_Tabelle__ + ex.Message,
                        Resources.HauptForm_UltraGridAfterRowInsert_Einfügefehler,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                this.RefreshDataSet(ref this.ultraGridBlutDruck, "BlutdruckDaten");   // Datensätze im DataAdapter auffrischen
            }
        }

        /// <summary>Bearbeitet das Einfügen einer Zeile .</summary>
        private void SliderScroll()
        {
            // Individuelle Bearbeitung je nach ausgewählter Ansicht
            if (this.selectedTab == (int)Ernaehrung)
            {
                // Es ist die Gewichts-Tabelle angewählt
                this.bindingManagerGewicht.Position = this.sliderErnaehrung.Value; // Datensatzposition der Gewichtsdaten auf die am Schieberegler eingestellte Position setzen

                // Aktive Zeile im Grid ermitteln
                this.indexNummerAktiveZeile = Convert.ToString(this.dataSetGewicht1.Tables["Gewicht"].Rows[this.bindingManagerGewicht.Position]["Index"]);
                this.LoescheAuswahl(this.ultraGridErnaehrung);                  // Zuerst alle Auswahlen zurücksetzen
                this.SetzeAuswahl(this.ultraGridErnaehrung);                    // Falls eine aktive Zeile existiert, diese anzeigen
                this.rowPosMerk = this.sliderErnaehrung.Value;                  // Jetzige Position im Datensatz merken
            }
            else
            {
                // Es ist die Blutdruck-Tabelle angewählt
                this.bindingManagerBlutDruck.Position = this.sliderBlutDruck.Value; // Datensatzposition der Blutdruckdaten auf die am Schieberegler eingestellte Position setzen

                // Aktive Zeile im Grid ermitteln
                this.indexNummerAktiveZeile = Convert.ToString(this.dataSetBlutDruck1.Tables["BlutdruckDaten"].Rows[this.bindingManagerBlutDruck.Position]["Index"]);
                this.LoescheAuswahl(this.ultraGridErnaehrung);                  // Zuerst alle Auswahlen zurücksetzen
                this.SetzeAuswahl(this.ultraGridErnaehrung);                    // Falls eine aktive Zeile existiert, diese anzeigen
                this.rowPosMerk = this.sliderErnaehrung.Value;                  // Jetzige Position im Datensatz merken
            }
        }
    }
}