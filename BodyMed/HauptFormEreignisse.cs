// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HauptFormEreignisseErnaehrung.Cs" company="Brenners Videotechnik">
//   Copyright (c) Brenners Videotechnik. All rights reserved.
// </copyright>
// <summary>
//   Zusammenfassung f�r HauptForm.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
// <remarks>
//     <para>Autor: Armin Brenner</para>
//     <para>
//        History : Datum     bearb.  �nderung
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
    /// Ereignisse f�r Blutdruckdaten.
    /// </summary>
    [SuppressMessage("ReSharper", "RedundantEmptyDefaultSwitchBranch")]
    [SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases")]
    public partial class HauptForm : RibbonForm
    {
        /// <summary>Bearbeitet die Aktivierung einer Zelle .</summary>
        private void UltraGridCellActivated()
        {
            // Individuelle Bearbeitung je nach ausgew�hlter Ansicht
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
        /// <param name="e">Die <see cref="EventArgs"/> Instanz, welche die Ereignisdaten enth�lt.</param>
        private void UltraGridAfterRowActivate(EventArgs e)
        {
            // Individuelle Bearbeitung je nach ausgew�hlter Ansicht
            if (this.selectedTab == (int)Ernaehrung)
            {
                // Abbrechen, wenn keine g�ltigen Daten vorhanden sind
                if (e == EventArgs.Empty || this.bindingManagerGewicht == null)
                {
                    return;                                                     // Abbruch, da keine Ereignisdaten �bergeben wurden oder keine Verbindungsdaten zur Datenbank vorhanden sind
                }

                this.blutDruckEinstellen = false;                               // Keine Blutdruckeingabe
                this.gewichtEinstellen = true;                                  // Gewichtseingabe ist aktiviert
                this.GetRowIndex();                                             // Index der aktiven Zeile ermitteln
                this.DisplayRecordNumbers();                                    // Nummer des aktuellen Datensatzes anzeigen ...
                this.rowPosMerkErnaehrung = this.sliderErnaehrung.Value;        // ... und merken
            }
            else
            {
                // Abbrechen, wenn keine g�ltigen Daten vorhanden sind
                if (e == EventArgs.Empty || this.bindingManagerBlutDruck == null)
                {
                    return;                                                     // Abbruch, da keine Ereignisdaten �bergeben wurden oder keine Verbindungsdaten zur Datenbank vorhanden sind
                }

                this.blutDruckEinstellen = true;                                // Blutdruckeingabe ist aktiviert
                this.gewichtEinstellen = false;                                 // keine Gewichtseingabe
                this.GetRowIndex();                                             // Index der aktiven Zeile ermitteln
                this.DisplayRecordNumbers();                                    // Nummer des aktuellen Datensatzes anzeigen ...
                this.rowPosMerkBlutDruck = this.sliderBlutDruck.Value;          // ... und merken
            }
        }

        /// <summary>Bearbeitet das Einf�gen einer Zeile .</summary>
        private void UltraGridAfterRowInsert()
        {
            this.auswahllisteLoeschen = true;                                   // Markierte Zeilen eines Grids d�rfen neu ermittelt werden
            string strInsert;                                                   // Variable f�r das Einf�gekommando


            // Individuelle Bearbeitung je nach ausgew�hlter Ansicht
            if (this.selectedTab == (int)Ernaehrung)
            {
                // Es ist die Gewichts-Tabelle angew�hlt
                // Es muss zuersrt die Gr�sse ermittelt werden . Die Gr�sse steht in der Textboc 'tbGroesse'
                var groesse = this.tbGroesse.Text;
                try
                {
                    strInsert = "INSERT INTO[Gewicht] ([Datum], [KG], [FM], [FFM], [KW], [Bmi], [Bemerkung], [Gr�sse])" 
                        + " VALUES("
                        + "'" + DateTime.Now + "'" + "," + "0.0 , 0.0, 0.0, 0.0, NULL, NULL, "
                        + groesse + ")";                                        // leeren Datensatz einf�gen

                    this.oleDbDataAdapterGewicht.InsertCommand.CommandText = strInsert;   // Einf�gekommando an DataAdapter �bergeben
                    this.oleDbDataAdapterGewicht.InsertCommand.ExecuteNonQuery();         // Einf�gen durchf�hren
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Resources.HauptForm_UltraGridAfterRowInsert_Fehler_beim_Einf�gen_eines_neuen_Datensatzes_in_die_Gewichtstabelle__ + ex.Message,
                        Resources.HauptForm_UltraGridAfterRowInsert_Einf�gefehler,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                this.RefreshDataSet(ref this.ultraGridErnaehrung, "Gewicht");   // Datens�tze im DataAdapter auffrischen
            }
            else
            {
                // Es ist die Blutdruck-Tabelle angew�hlt
                try
                {
                    strInsert = "INSERT INTO[BlutdruckDaten] ([Datum], [Systolisch], [Diastolisch], [Puls])" + " VALUES("
                            + "'" + DateTime.Now + "'" + "," + "0 , 0, 0)";     // leeren Datensatz einf�gen

                    this.oleDbDataAdapterBlutDruck.InsertCommand.CommandText = strInsert; // Einf�gekommando an DataAdapter �bergeben
                    this.oleDbDataAdapterBlutDruck.InsertCommand.ExecuteNonQuery();       // Einf�gen durchf�hren
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Resources.HauptForm_UltraGridAfterRowInsert_Fehler_beim_Einf�gen_eines_neuen_Datensatzes_in_die_Butdruck_Tabelle__ + ex.Message,
                        Resources.HauptForm_UltraGridAfterRowInsert_Einf�gefehler,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                this.RefreshDataSet(ref this.ultraGridBlutDruck, "BlutdruckDaten");   // Datens�tze im DataAdapter auffrischen
            }
        }

        /// <summary>Bearbeitet das Einf�gen einer Zeile .</summary>
        private void SliderScroll()
        {
            // Individuelle Bearbeitung je nach ausgew�hlter Ansicht
            if (this.selectedTab == (int)Ernaehrung)
            {
                // Es ist die Gewichts-Tabelle angew�hlt
                this.bindingManagerGewicht.Position = this.sliderErnaehrung.Value; // Datensatzposition der Gewichtsdaten auf die am Schieberegler eingestellte Position setzen

                // Aktive Zeile im Grid ermitteln
                this.indexNummerAktiveZeile = Convert.ToString(this.dataSetGewicht1.Tables["Gewicht"].Rows[this.bindingManagerGewicht.Position]["Index"]);
                this.LoescheAuswahl(this.ultraGridErnaehrung);                  // Zuerst alle Auswahlen zur�cksetzen
                this.SetzeAuswahl(this.ultraGridErnaehrung);                    // Falls eine aktive Zeile existiert, diese anzeigen
                this.rowPosMerk = this.sliderErnaehrung.Value;                  // Jetzige Position im Datensatz merken
            }
            else
            {
                // Es ist die Blutdruck-Tabelle angew�hlt
                this.bindingManagerBlutDruck.Position = this.sliderBlutDruck.Value; // Datensatzposition der Blutdruckdaten auf die am Schieberegler eingestellte Position setzen

                // Aktive Zeile im Grid ermitteln
                this.indexNummerAktiveZeile = Convert.ToString(this.dataSetBlutDruck1.Tables["BlutdruckDaten"].Rows[this.bindingManagerBlutDruck.Position]["Index"]);
                this.LoescheAuswahl(this.ultraGridErnaehrung);                  // Zuerst alle Auswahlen zur�cksetzen
                this.SetzeAuswahl(this.ultraGridErnaehrung);                    // Falls eine aktive Zeile existiert, diese anzeigen
                this.rowPosMerk = this.sliderErnaehrung.Value;                  // Jetzige Position im Datensatz merken
            }
        }
    }
}