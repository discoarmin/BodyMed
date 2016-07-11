// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HauptForm.Cs" company="Brenners Videotechnik">
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
//                  02.04.16  br      Grundversion
//      </para>
// </remarks>
// --------------------------------------------------------------------------------------------------------------------
namespace BodyMed
{
    using System;
    using System.Windows.Forms;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Reflection;

    using Infragistics.Win.UltraWinGrid;

    using Microsoft.VisualBasic.ApplicationServices;

    using static HauptForm.Formular;

    using Resources = BodyMed.Properties.Resources;

    //using ZedGraph;

    [SuppressMessage("ReSharper", "RedundantEmptyDefaultSwitchBranch")]
    [SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases")]
    public partial class HauptForm
    {

        public HauptForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Haptform geladen wird eintritt.
        /// </summary>
        /// <param name="sender">Das aufrufende Element</param>
        /// <param name="e">Die <see cref="System.EventArgs" /> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnHauptFormLoad(object sender, EventArgs e)
        {
            // Versions-Nr. in die Stausbar eintragen
            var assemblyInfoMainApplication = new AssemblyInfo(Assembly.GetEntryAssembly());

            this.statusBar.Panels["Version"].Text = !string.IsNullOrEmpty(assemblyInfoMainApplication.ProductName)
                                      ? string.Concat(
                                        assemblyInfoMainApplication.ProductName,
                                        "  Ver. ",
                                        assemblyInfoMainApplication.Version)
                                      : "Nicht ermittelbar";                    // Version in der Statusbar eintragen


            this.LadeDatenBank();                                               // Datenbank bereitstellen
            this.selectedTab = 0;                                               // Gewichtseingabe ist beim Start aktiv
            this.gewichtEinstellen = true;                                      // Position bei den Ernährungsdaten darf geändert werden
            this.blutDruckEinstellen = true;                                    // Position bei den Blutdruckdaten darf geändert werden
            this.auswahllisteLoeschen = true;                                   // Markierte Zeilen eines Grids dürfen neu ermittelt werden
            // Die Buttons zur Fensterauswahl sollen bei einer Auswahl markiert werden
            this.ribbonButtonEingabe.CheckOnClick = true;
            this.ribbonButtonBlutdruck.CheckOnClick = true;
            this.ribbonButtonEingabe.PerformClick();                            // Button 'Eingabe' betätigen
        }

        /// <summary>
        /// Behandelt das ToolClick-Ereignis des  toolbarsManager Controls.
        /// </summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="EventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnRibbonButtonClick(object sender, EventArgs e)
        {
            var btn = (RibbonButton)sender;                                     // Der betätigte Button

            // Tastendruck oder Mausklick auf einen Menüpunkt auswerten
            if (btn.Tag == null) return;                                        // Abbruch, wenn es keinen Tag gibbt
            switch (btn.Tag.ToString())
            {
                default:
//                    Application.DoEvents();
                    break;
                case "Eingabe":                                                 // Ernährungsdaten eingaben
                    this.SetzeAufErnaehrung();
                    this.statusBar.Panels["Fenster"].Text = btn.Tag.ToString(); // Ausgewähltes Fenster in der Statusleiste anzeigen

                    // Für den ausgewählten Button wird das Flaschen abgeschaltet
                    this.ribbonButtonEingabe.FlashEnabled = false;
                    this.ribbonButtonBlutdruck.Checked = false;
                    this.ribbonButtonBlutdruck.FlashEnabled = true;
                    this.ribbonButtonBlutdruck.FlashIntervall = 1000;
                    break;
                case "Blutdruck":                                               // Blutdruckdaten eingaben
                    this.SetzeAufBlutDruck();
                    this.statusBar.Panels["Fenster"].Text = btn.Tag.ToString(); // Ausgewähltes Fenster in der Statusleiste anzeigen

                    // Für den ausgewählten Button wird das Flaschen abgeschaltet
                    this.ribbonButtonEingabe.FlashEnabled = true;
                    this.ribbonButtonBlutdruck.FlashEnabled = false;
                    this.ribbonButtonEingabe.FlashIntervall = 1000;
                    this.ribbonButtonEingabe.Checked = false;

                    break;
                case "Neu":                                                     // Neuer Datensatz
                    // Zuerst ermitteln, welche Eingabe aktiv ist
                    switch (this.selectedTab)
                    {
                        case 0:                                                 // Gewichtseingabe
                            this.ultraGridErnaehrung.Rows.Band.AddNew();        // Neuen Datensatz hizufügen
                            break;
                        case 1:                                                 // Eingabe Blutdruckdaten
                            this.ultraGridBlutDruck.Rows.Band.AddNew();         // Neuen Datensatz hizufügen
                            break;
                    }

                    break;
            }
            Application.DoEvents();

        }

        /// <summary>Zeigt die Daten für die Gewichtseingabe an.</summary>
        private void SetzeAufErnaehrung()
        {
            this.ultraTabControlHauptForm.Tabs["Ernaehrung"].Selected = true;   // Eingabe der Gewichtsdaten
            this.selectedTab = this.ultraTabControlHauptForm.SelectedTab.Index; // Nummer des ausgewählten Tabs merken
            this.DisplayRecordNumbers();                                        // Anzahl Datensätze in der Statusleiste anzeigen
        }

        /// <summary>Zeigt die Daten für die Blutdruckeingabe an.</summary>
        private void SetzeAufBlutDruck()
        {
            this.ultraTabControlHauptForm.Tabs["BlutDruck"].Selected = true;    // Eingabe der Blutdruckdaten
            this.selectedTab = this.ultraTabControlHauptForm.SelectedTab.Index; // Nummer des ausgewählten Tabs merken
            this.DisplayRecordNumbers();                                        // Anzahl Datensätze in der Statusleiste anzeigen
        }

        /// <summary> Manager für Datenbankanbindungen zu den einzelnen Tabellen der Datenbank bereitstellen. </summary>
        private void LadeDatenBank()
        {
            this.GetDataConnection();                                           // Verbindung zur Datenbank herstellen


            this.bindingManagerGewicht = this.BindingContext[this.dataSetGewicht1.Tables["Gewicht"]];
            this.bindingManagerBlutDruck = this.BindingContext[this.dataSetBlutDruck1.Tables["BlutdruckDaten"]];

            this.tbGroesse.DataBindings.Add("Text", this.dataSetGroesse1.Tables["Groesse"], "Groesse");

            // Spalte 'Index' soll in beiden Grids nicht angezeigt werden
            this.ultraGridErnaehrung.DisplayLayout.Bands[0].Columns["Index"].Hidden = true;
            this.ultraGridBlutDruck.DisplayLayout.Bands[0].Columns["Index"].Hidden = true;
            //     DataBindings.Add("Text", this.bindingSourceGroesse, "Groesse");
        }


        /// <summary>
        /// Behandelt das ResizeEnd-Ereignis des HauptForm-Controls.
        /// </summary>
        /// <param name="sender">Das aufrufende Element.</param>
        /// <param name="e">Die <see cref="System.EventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnHauptFormResizeEnd(object sender, EventArgs e)
        {
            // Formular neu zeichnen
            this.Invalidate(true);
            this.Update();
        }

        /// <summary>
        /// Behandelt das Resize-Ereignis des splitContainerHauptForm-Controls.
        /// </summary>
        /// <param name="sender">Das aufrufende Element.</param>
        /// <param name="e">Die <see cref="System.EventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnSplitContainerHauptFormResize(object sender, EventArgs e)
        {
            this.splitContainerHauptForm.SplitterDistance = 136;
        }

        /// <summary>
        /// Verbindung zur Datenbank ermitteln.
        /// </summary>
        /// <exception cref="Exception">Wenn Verbindung fehlgeschlagen ist</exception>
        private void GetDataConnection()
        {
            try
            {
                this.oleDbConnection1.Open();                                   // Verbindung zur Datenbank öffnen
                if (this.oleDbConnection1.State == ConnectionState.Open)
                {
                    // Beide Grids mit Daten füllen
                    this.dataSetGewicht1.Tables["Gewicht"].Clear();             // Inhalt des Datensatzes für Gewichtsdaten löschen..
                    this.oleDbDataAdapterGewicht.Fill(this.dataSetGewicht1.Tables["Gewicht"]); // .. und neue Daten einlesen
                    this.dataSetBlutDruck1.Tables["BlutdruckDaten"].Clear();    // Inhalt des Datensatzes für Blutdruck löschen..
                    this.oleDbDataAdapterBlutDruck.Fill(this.dataSetBlutDruck1.Tables["BlutdruckDaten"]); // .. und neue Daten einlesen

                    this.dataSetGroesse1.Tables["Groesse"].Clear();             // Inhalt des Datensatzes für die Größe löschen..
                    this.oleDbDataAdapterGroesse.Fill(this.dataSetGroesse1.Tables["Groesse"]); // .. und neue Daten einlesen
                }

            }
            catch (Exception ex)
            {
                // Aufgetretene Ausnahme anzeigen
                MessageBox.Show(
                    Resources.HauptForm_GetDataConnection_Fehler__ + ex.Message,
                    Resources.HauptForm_GetDataConnection_Verbindung_zur_Datenbankfehlgeschlagen,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Application.Exit(); // Programm beenden
            }
        }

        #region DatasetAction

        /// <summary>
        /// Bestimme die momentane Position im Datensatz und zeige sie in der Statusbar an
        /// </summary>
        private void DisplayRecordNumbers()
        {
            // Position im Bindmanager darf geändert werden
            this.blutDruckEinstellen = true;
            this.gewichtEinstellen = true;
            // Überprüfen, welche Ansicht angewählt ist
            switch (this.selectedTab)
            {
                case (int)Ernaehrung:
                    {
                        // Ansicht zur Eingabe des Gewichts ist angewählt
                        if (this.bindingManagerGewicht != null)
                        {
                            // Ernährungsdaten
                            if (this.rowIndex >= 0)
                            {
                                try
                                {
                                    this.statusBar.Panels["tcurrentDirectory"].Text =
                                        Resources.HauptForm_DisplayRecordNumbers_Ernährung__Datensatz_
                                        + (this.rowIndex + 1) + Resources.HauptForm_DisplayRecordNumbers__von_
                                        + this.bindingManagerGewicht.Count
                                        + Resources.HauptForm_DisplayRecordNumbers__;
                                }
                                catch
                                {
                                    this.gewichtEinstellen = false;             // Position im Bindmanager nicht verstellen
                                    this.statusBar.Panels["tcurrentDirectory"].Text =
                                        Resources.HauptForm_DisplayRecordNumbers_Ernährung__Datensatz_
                                        + this.rowPos + Resources.HauptForm_DisplayRecordNumbers__von_
                                        + this.bindingManagerGewicht.Count
                                        + Resources.HauptForm_DisplayRecordNumbers__;
                                }
                            }
                            else
                            {
                                this.gewichtEinstellen = false;                 // Position im Bindmanager nicht verstellen
                                this.statusBar.Panels["tcurrentDirectory"].Text =
                                    Resources.HauptForm_DisplayRecordNumbers_Ernährung__Datensatz_ + this.rowPos
                                    + Resources.HauptForm_DisplayRecordNumbers__von_ + this.bindingManagerGewicht.Count;
                            }

                            // Schieberegler einstellen
                            if (this.bindingManagerGewicht.Count > 0)
                            {
                                this.sliderErnaehrung.Maximum = this.bindingManagerGewicht.Count - 1; // Maximalwert des Sliders festlegen
                            }
                        }

                        // Slider auf ausgewählte Position stellen
                        // Wenn kein Datensatz vorhanden ist, Slider ausblenden
                        this.sliderErnaehrung.Visible = false;
                        if (this.rowIndex >= 0)
                        {
                            // Falls der Index höher ist als die Anzahl Zeilen (kommt nach dem Löschen vor),
                            // auf den Maximalwert setzen
                            if (this.rowIndex > this.sliderErnaehrung.Maximum)
                            {
                                this.rowIndex = this.sliderErnaehrung.Maximum;
                            }

                            this.sliderErnaehrung.Value = this.rowIndex;
                            this.sliderErnaehrung.Visible = true;
                        }
                        else
                        {
                            // Abfragen, ob Position im Bindmanager verändert werden darf
                            if (this.gewichtEinstellen)
                            {
                                // Position im Bindmanager darf verstellt werden
                                this.sliderErnaehrung.Value = this.sliderErnaehrung.Minimum;
                                this.sliderErnaehrung.Visible = false;
                            }
                        }
                        break;
                    }

                case (int)BlutDruck:
                    {
                        // Ansicht zur Eingabe des Blutdrucks ist angewählt
                        if (this.bindingManagerBlutDruck != null)
                        {
                            // Blutdruck-Daten
                            if (this.rowIndex >= 0)
                            {
                                try
                                {
                                    // Datensatzinfo in der Statuszeile anzeigen
                                    this.statusBar.Panels["tcurrentDirectory"].Text =
                                        Resources.HauptForm_DisplayRecordNumbers_Blutdruck__Datensatz__
                                        + (this.rowIndex + 1) + Resources.HauptForm_DisplayRecordNumbers__von_
                                        + this.bindingManagerBlutDruck.Count
                                        + Resources.HauptForm_DisplayRecordNumbers__;
                                }
                                catch
                                {
                                    this.blutDruckEinstellen = false;           // Position im Bindmanager nicht verstellen
                                    this.statusBar.Panels["tcurrentDirectory"].Text =
                                        Resources.HauptForm_DisplayRecordNumbers_Ernährung__Datensatz_
                                        + this.rowPos + Resources.HauptForm_DisplayRecordNumbers__von_
                                        + this.bindingManagerBlutDruck.Count
                                        + Resources.HauptForm_DisplayRecordNumbers__;
                                }
                            }
                            else
                            {
                                this.blutDruckEinstellen = false;               // Position im Bindmanager nicht verstellen
                                this.statusBar.Panels["tcurrentDirectory"].Text =
                                    Resources.HauptForm_DisplayRecordNumbers_Ernährung__Datensatz_
                                    + this.rowPos + Resources.HauptForm_DisplayRecordNumbers__von_
                                    + this.bindingManagerBlutDruck.Count
                                    + Resources.HauptForm_DisplayRecordNumbers__;
                            }

                            // Schieberegler einstellen
                            if (this.bindingManagerGewicht.Count > 0)
                            {
                                this.sliderBlutDruck.Maximum = this.bindingManagerBlutDruck.Count - 1; // Maximalwert des Sliders festlegen
                            }
                        }

                        // Slider auf ausgewählte Position stellen
                        // Wenn kein Datensatz vorhanden ist, Slider ausblenden
                        this.sliderBlutDruck.Visible = false;
                        if (this.rowIndex > 0)
                        {
                            // Falls der Index höher ist als die Anzahl Zeilen (kommt nach dem Löschen vor),
                            // auf den Maximalwert setzen
                            if (this.rowIndex > this.sliderBlutDruck.Maximum)
                            {
                                this.rowIndex = this.sliderBlutDruck.Maximum;
                            }

                            this.sliderBlutDruck.Value = this.rowIndex;
                            this.sliderBlutDruck.Visible = true;
                        }
                        else
                        {
                            // Abfragen, ob Position im Bindmanager verändert werden darf
                            if (this.blutDruckEinstellen)
                            {
                                this.sliderBlutDruck.Value = this.sliderBlutDruck.Minimum;
                                this.sliderBlutDruck.Visible = false;
                            }
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Index der aktiven Zeile ermitteln.
        /// </summary>
        private void GetRowIndex()
        {
            var grid = this.ultraGridErnaehrung;                                // Momentan aktives Grid
            var bindingManager = this.bindingManagerGewicht;                    // Verwaltet die Datenanbindung
            var tabelle = string.Empty;                                         // Name der Tabelle
            DataSet ds = this.dataSetGewicht1;                                  // DataSet mit welchem gearbveitet wird

            // Überprüfen,Tabelle angewählt ist zum Einstellen der nötigen Variablen
            switch (this.selectedTab)
            {
                case (int)Ernaehrung:
                    {
                        grid = this.ultraGridErnaehrung;                        // Grid auswählen
                        bindingManager = this.bindingManagerGewicht;            // Verwaltet die Gewichts-Daten
                        tabelle = "Gewicht";                                    // Name der Tabelle
                        ds = this.dataSetGewicht1;                              // Zugehöriges DataSet
                        break;
                    }

                case (int)BlutDruck:
                    {
                        grid = this.ultraGridBlutDruck;                         // Grid auswählen
                        bindingManager = this.bindingManagerBlutDruck;          // Verwaltet die Blutdruck-Daten
                        tabelle = "BlutdruckDaten";                             // Name der Tabelle
                        ds = this.dataSetBlutDruck1;                            // Zugehöriges DataSet
                        break;
                    }
            }

            if (bindingManager.Position < ds.Tables[tabelle].Rows.Count && bindingManager.Position >= 0)
            {
                var aktIndex = grid.ActiveRow.Index;                            // Index der aktiven Zeile im Grid ermitteln
                this.indexNummerAktiveZeile = grid.Rows[aktIndex].Cells[0].Value.ToString(); // Zugehöriger Wert der Spalte 'Index' im Datensatz

                // Index im Bindmanager suchen, dazu alle Zeilen durchgehen
                for (var pos = 0; pos < bindingManager.Count; pos++)
                {
                    bindingManager.Position = pos;                              // Auf eine Zeile im Bindmanager positionieren

                    // Überprüfen, ob es sich um eine gelöschte Zeile handelt. Auf gelöschte Zeilen kann nicht zugegriffen werden
                    if (ds.Tables[tabelle].Rows[pos].RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    var indexTest = Convert.ToInt32(ds.Tables[tabelle].Rows[bindingManager.Position][0]); // Index des Datensatzes ermitteln

                    // Indizes vergleichen. Ist der BindingManager inaktiv, ist die Position negativ
                    if (indexTest != Convert.ToInt32(this.indexNummerAktiveZeile) || bindingManager.Position < 0)
                    {
                        continue;                                               // Index nicht gefunden, also weitersuchen
                    }

                    // Index des gesuchten Datensatzes gefunden, Position im Bindingmanager für Übertragung merken
                    this.rowIndex = bindingManager.Position;
                    this.indexNummer = ds.Tables[tabelle].Rows[bindingManager.Position][0].ToString();
                }
            }

            this.DisplayRecordNumbers();
        }
        #endregion DatasetAction

        #region ultraGridErnaehrung
        /// <summary>
        /// Behandelt das 'AfterCellActivate' Ereignis des ultraGridErnaehrung Controls.
        /// </summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="EventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridErnaehrungAfterCellActivate(object sender, EventArgs e)
        {
            this.UltraGridCellActivated();
        }

        /// <summary>
        /// Behandelt das 'AfterRowActivate' Ereignis des ultraGridErnaehrung Controls.
        /// </summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="EventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridErnaehrungAfterRowActivate(object sender, EventArgs e)
        {
            this.UltraGridAfterRowActivate(e);
        }

        /// <summary>
        /// Behandelt das 'AfterExitEditMode' Ereignis des ultraGridErnaehrung Controls.
        /// </summary>
        /// Der Editiermodus im ultraGridErnaehrung wurde beendet
        private void OnUltraGridErnaehrungAfterExitEditMode(object sender, EventArgs e)
        {
            this.AfterExitEditMode(ref this.ultraGridErnaehrung, "Gewicht");    // Änderungen in Datenbank schreiben
        }

        /// <summary>Behandelt das AfterRowInsert Ereignis des ultraGridErnaehrung Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridErnaehrungAfterRowInsert(object sender, RowEventArgs e)
        {
            this.UltraGridAfterRowInsert();
        }

        /// <summary>Behandelt das AfterRowDeleted Ereignis des ultraGridErnaehrung Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridErnaehrungAfterRowsDeleted(object sender, EventArgs e)
        {

        }

        /// <summary>Behandelt das AfterSelectChange Ereignis des ultraGridErnaehrung Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridErnaehrungAfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {

        }

        /// <summary>Behandelt das AfterSortChange Ereignis des ultraGridErnaehrung Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridErnaehrungAfterSortChange(object sender, BandEventArgs e)
        {

        }

        /// <summary>Behandelt das BeforeRowsDeleted Ereignis des ultraGridErnaehrung Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridErnaehrungBeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {

        }

        /// <summary>Behandelt das CellChange Ereignis des ultraGridErnaehrung Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridErnaehrungCellChange(object sender, CellEventArgs e)
        {

        }

        /// <summary>Behandelt das InitializeLayout Ereignis des ultraGridErnaehrung Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridErnaehrungInitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

        }

        /// <summary>Behandelt das InitializePrint Ereignis des ultraGridErnaehrung Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridErnaehrungInitializePrint(object sender, CancelablePrintEventArgs e)
        {

        }

        /// <summary>Behandelt das Leave Ereignis des ultraGridErnaehrung Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridErnaehrungLeave(object sender, EventArgs e)
        {

        }
        #endregion

        #region ultraGridBlutDruck

        /// <summary>
        /// Behandelt das 'AfterCellActivate' Ereignis des ultraGridBlutDruck Controls.
        /// </summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="EventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridBlutDruckAfterCellActivate(object sender, EventArgs e)
        {
            this.UltraGridCellActivated();
        }

        /// <summary>
        /// Behandelt das 'AfterExitEditMode' Ereignis des ultraGridBlutDruck Controls.
        /// </summary>
        /// Der Editiermodus im ultraGridBlutDruck wurde beendet
        private void OnUltraGridBlutDruckAfterExitEditMode(object sender, EventArgs e)
        {
            this.AfterExitEditMode(ref this.ultraGridBlutDruck, "BlutdruckDaten"); // Änderungen in Datenbank schreiben
        }

        /// <summary>Behandelt das AfterRowInsert Ereignis des ultraGridBlutDruck Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridBlutDruckAfterRowInsert(object sender, RowEventArgs e)
        {
            this.UltraGridAfterRowInsert();
        }

        /// <summary>Behandelt das AfterRowDeleted Ereignis des ultraGridBlutDruck Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridBlutDruckAfterRowsDeleted(object sender, EventArgs e)
        {

        }

        /// <summary>Behandelt das AfterSelectChange Ereignis des ultraGridBlutDruck Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridBlutDruckAfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {

        }

        /// <summary>Behandelt das AfterSortChange Ereignis des ultraGridBlutDruck Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridBlutDruckAfterSortChange(object sender, BandEventArgs e)
        {

        }

        /// <summary>Behandelt das BeforeRowsDeleted Ereignis des ultraGridBlutDruck Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraultraGridBlutDruckBeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
        {

        }

        /// <summary>Behandelt das CellChange Ereignis des ultraGridBlutDruck Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridBlutDruckCellChange(object sender, CellEventArgs e)
        {

        }

        /// <summary>Behandelt das InitializeLayout Ereignis des ultraGridBlutDruck Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridBlutDruckInitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

        }

        /// <summary>Behandelt das InitializePrint Ereignis des ultraGridBlutDruck Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OultraGridBlutDruckInitializePrint(object sender, CancelablePrintEventArgs e)
        {

        }

        /// <summary>Behandelt das Leave Ereignis des ultraGridBlutDruck Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridBlutDruckLeave(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Behandelt das 'AfterRowActivate' Ereignis des ultraGridBlutDruck Controls.
        /// </summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="EventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridBlutDruckAfterRowActivate(object sender, EventArgs e)
        {
            this.UltraGridAfterRowActivate(e);
        }

        #endregion

        #region Navigation

        /// <summary>
        /// Wird aufgerufen, wenn der Schieberegler zum Positionieren im Datensatz bewegt wird.
        /// </summary>
        /// <param name="sender">Das aufrufende Element.</param>
        /// <param name="e">Die <see cref="System.EventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnSliderScroll(object sender, EventArgs e)
        {
            this.SliderScroll();
        }
        #endregion

        /// <summary>
        /// Wird aufgerufen, wenn der aktive Tab im Ribbon gewechselt wird.
        /// </summary>
        /// <param name="sender">Das aufrufende Element.</param>
        /// <param name="e">Die <see cref="System.EventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnRibbon1ActiveTabChanged(object sender, EventArgs e)
        {
            Application.DoEvents();
        }

        /// <summary>
        /// Wird aufgerufen, wenn im ultraGridErnaehrung eine Zelle deaktiviert wurde.
        /// </summary>
        /// <param name="sender">Das aufrufende Element.</param>
        /// <param name="e">Die <see cref="System.EventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridErnaehrungBeforeCellDeactivate(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var grid = (UltraGrid)sender;                                       // aufrufendes Element ist ein Ultragrid

            // Spaltenname der aktiven Zelle überprüfen. Ist die Spalte das Gewicht,
            // kann der Bmi-Wert berechnet werden, sonst muss nichts getan werden
            if (grid.ActiveCell.Column.Key != "KG")
            {
                return;                                                         // Abbruch, da kein Gewichtswert
            }

            // Gewicht und Grösse ermitteln
            var gewicht = grid.ActiveCell.Text;                                 // Gewicht
            var groesse = this.tbGroesse.Text;                                  // Grösse
            string bmiDescription;                                              // Klassifizierung des errechneten Bmi-Wertes
            double bmi;                                                         // Der errechnete BMI-Wert
            Color farbe;                                                        // Hintergrundfarbe anhand der Klassifizierung

            // Bmi-Wert berechnen und klassifizieren
           BerechneBmi(gewicht, groesse, out bmi, out bmiDescription, out farbe);

            // BMI-Wert und dessen Beschreibung in die Datenbank eintragen
            var zeile = grid.DisplayLayout.ActiveRow;                           // Nummer der Zeile ermitteln
            var spalte = grid.DisplayLayout.Bands["Gewicht"].Columns["BMI"].Index; // Nummer der Spalte für den BMI-Wert
            grid.ActiveCell = zeile.Cells[spalte];
            grid.ActiveCell.Value = bmi;                                        // BMI-Wert eintragen
            grid.DisplayLayout.Bands["Gewicht"].Columns["BMI"].CellAppearance.BackColor = farbe; // Hintergrundfarbe je nach Klassifizierung des BMI-Werts einstellen

            spalte = grid.DisplayLayout.Bands["Gewicht"].Columns["Bemerkung"].Index; // Nummer der Spalte für die Beschreibung des BMI-Werts
            grid.ActiveCell = zeile.Cells[spalte];
            grid.ActiveCell.Value = bmiDescription;                             // Beschreibung des BMI-Werts eintragen
            this.AfterExitEditMode(ref grid, "Gewicht");                        // Damit die Datenbank aufgefrischt wird
        }

         private void RibbonTabFensterMouseEnter(object sender, MouseEventArgs e)
        {
            // Placebo-Funktion
        }

        /// <summary>Wird aufgerufen, wenn die Hauptform geschlossen wurde. </summary>
        /// <param name="sender">Das aufrufende Element</param>
        /// <param name="e">Die <see cref="FormClosedEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnHauptFormFormClosed(object sender, FormClosedEventArgs e)
        {
            this.DatenbankVerbindungSchliessen();                               // Schließt die Verbindung zur Datenbank

            // Speicher bereinigen
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void OnHauptFormFormClosed(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}