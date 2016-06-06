namespace BodyMed
{
    using System;
    using System.Windows.Forms;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;

    using Infragistics.Win.UltraWinGrid;

    using static HauptForm.Formular;

    using Resources = BodyMed.Properties.Resources;

    //using ZedGraph;

    [SuppressMessage("ReSharper", "RedundantEmptyDefaultSwitchBranch")]
    [SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases")]
    public partial class HauptForm : RibbonForm
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
            this.LadeDatenBank();
            this.selectedTab = 1;                                               // Gewichtseingabe ist beim Start aktiv
            this.gewichtEinstellen = true;                                      // Position bei den Ernährungsdaten darf geändert werden
            this.blutDruckEinstellen = true;                                    // Position bei den Blutdruckdaten darf geändert werden
        }

        /// <summary>
        /// Behandelt das ToolClick-Ereignis des  toolbarsManager Controls.
        /// </summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="EventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnRibbonButtonClick(object sender, EventArgs e)
        {
            string anzeigeText;
            var btn = (RibbonButton)sender; // Der betätigte Button

            // Tastendruck oder Mausklick auf einen Menüpunkt auswerten
            switch (btn.Tag.ToString())
            {
                default:
                    break;
                case "Eingabe": // Ernährungsdaten eingaben
                    this.SetzeAufErnaehrung();
                    break;
                case "Blutdruck": // Blutdruckdaten eingaben
                    this.SetzeAufBlutDruck();
                    break;
                case "Neu": // Neuer Datensatz
                    // Zuerst ermitteln, welche Eingabe aktiv ist
                    switch (this.selectedTab)
                    {
                        case 1: // Gewichtseingabe
                            this.ultraGridErnaehrung.Rows.Band.AddNew(); // Neuen Datensatz hizufügen
                            break;
                        case 2: // Eingabe Blutdruckdaten
                            this.ultraGridBlutDruck.Rows.Band.AddNew(); // Neuen Datensatz hizufügen
                            break;
                    }

                    break;
            }
        }

        /// <summary>Zeigt die Daten für die Gewichtseingabe an.</summary>
        private void SetzeAufErnaehrung()
        {
            this.ultraTabControlHauptForm.Tabs["Ernaehrung"].Selected = true; // Eingabe der Gewichtsdaten
            this.selectedTab = this.ultraTabControlHauptForm.SelectedTab.Index; // Nummer des ausgewählten Tabs merken
        }

        /// <summary>Zeigt die Daten für die Blutdruckeingabe an.</summary>
        private void SetzeAufBlutDruck()
        {
            this.ultraTabControlHauptForm.Tabs["BlutDruck"].Selected = true; // Eingabe der Blutdruckdaten
            this.selectedTab = this.ultraTabControlHauptForm.SelectedTab.Index; // Nummer des ausgewählten Tabs merken
        }

        /// <summary> Manager für Datenbankanbindungen zu den einzelnen Tabellen der Datenbank bereitstellen. </summary>
        private void LadeDatenBank()
        {
            this.GetDataConnection(); // Verbindung zur Datenbank herstellen


            this.bindingManagerGewicht = this.BindingContext[this.dataSetGewicht1.Tables["Gewicht"]];
            this.bindingManagerBlutDruck = this.BindingContext[this.dataSetBlutDruck1.Tables["BlutdruckDaten"]];

            this.tbGroesse.DataBindings.Add("Text", this.dataSetGroesse1.Tables["Groesse"], "Groesse");
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
                this.oleDbConnection1.Open(); // Verbindung zur Datenbank öffnen
                if (this.oleDbConnection1.State == ConnectionState.Open)
                {
                    // Beide Grids mit Daten füllen
                    this.dataSetGewicht1.Tables["Gewicht"].Clear();
                        // Inhalt des Datensatzes für Gewichtsdaten löschen..
                    this.oleDbDataAdapterGewicht.Fill(this.dataSetGewicht1.Tables["Gewicht"]);
                        // .. und neue Daten einlesen
                    this.dataSetBlutDruck1.Tables["BlutdruckDaten"].Clear();
                        // Inhalt des Datensatzes für Blutdruck löschen..
                    this.oleDbDataAdapterBlutDruck.Fill(this.dataSetBlutDruck1.Tables["BlutdruckDaten"]);
                        // .. und neue Daten einlesen

                    this.dataSetGroesse1.Tables["Groesse"].Clear(); // Inhalt des Datensatzes für die Größe löschen..
                    this.oleDbDataAdapterGroesse.Fill(this.dataSetGroesse1.Tables["Groesse"]);
                        // .. und neue Daten einlesen
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

        /// <summary>
        /// Behandelt das 'AfterExitEditMode' Ereignis des ultraGridErnaehrung Controls.
        /// </summary>
        /// Der Editiermodus im ultraGridMotor wurde beendet
        private void OnUltraGridErnaehrungAfterExitEditMode(object sender, EventArgs e)
        {
            this.AfterExitEditMode(ref this.ultraGridErnaehrung, "Gewicht"); // Änderungen in Datenbank schreiben
        }

        #region DatasetAction

        /// <summary>
        /// Bestimme die momentane Position im Datensatz und zeige sie in der Statusbar an
        /// </summary>
        private void DisplayRecordNumbers()
        {
            var nichtAendern = true;                                            // Position im Bindmanager darf geändert werden

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
                        if (rowIndex >= 0)
                        {
                            // Falls der Index höher ist als die Anzahl Zeilen (kommt nach dem Löschen vor),
                            // auf den Maximalwert setzen
                            if (rowIndex > sliderFlexTool.Maximum)
                            {
                                rowIndex = sliderFlexTool.Maximum;
                            }

                            sliderFlexTool.Value = rowIndex;
                            sliderFlexTool.Visible = true;
                            this.ultraNumericEditorNavigation.Value = rowIndex;
                            this.ultraNumericEditorNavigation.Visible = true;

                        }
                        else
                        {
                            // Abfragen, ob Position im Bindmanager verändert werden darf
                            if (reglerEinstellen)
                            {
                                // Position im Bindmanager darf verstellt werden
                                this.sliderFlexTool.Value = this.sliderFlexTool.Minimum;
                                this.sliderFlexTool.Visible = false;
                                this.ultraNumericEditorNavigation.Value = this.sliderFlexTool.Minimum;
                                this.ultraNumericEditorNavigation.Visible = false;
                            }
                        }
                        break;
                    }

                case (ushort)Werkzeug.FuegeModul:
                    {
                        // Werkzeug ist ein Fügemodul
                        if (bindManagerFuegeModul != null)
                        {
                            // Fügemoduldaten
                            if (rowIndex >= 0)
                            {
                                try
                                {
                                    // Datensatzinfo in der Statuszeile anzeigen
                                    if (!LeeresWerkZeug)
                                    {
                                        statusBar.Panels["tcurrentDirectory"].Text =
                                            Resources.HauptForm_DisplayRecordNumbers_Fügemodul_Datensatz____
                                            + (this.rowIndex + 1) + Resources.HauptForm_DisplayRecordNumbers__von__
                                            + this.bindManagerFuegeModul.Count
                                            + Resources.HauptForm_DisplayRecordNumbers_
                                            + this.datasetFuegeModul.Tables["Chipinfos"].Rows[rowIndex][
                                                "Seriennummer Spindel"];
                                    }
                                    else
                                    {
                                        statusBar.Panels["tcurrentDirectory"].Text =
                                            Resources.HauptForm_DisplayRecordNumbers_Fügemodul_Datensatz____
                                            + (this.rowIndex + 1) + Resources.HauptForm_DisplayRecordNumbers__von__
                                            + this.bindManagerFuegeModul.Count
                                            + Resources.HauptForm_DisplayRecordNumbers_;
                                    }
                                }
                                catch
                                {
                                    reglerEinstellen = false; // Position im Bindmanager nicht verstellen
                                    statusBar.Panels["tcurrentDirectory"].Text =
                                        Resources.HauptForm_DisplayRecordNumbers_Fügemodul_Datensatz____ + this.rowPos
                                        + Resources.HauptForm_DisplayRecordNumbers__von__
                                        + this.bindManagerFuegeModul.Count;
                                }
                            }
                            else
                            {
                                reglerEinstellen = false; // Position im Bindmanager nicht verstellen
                                statusBar.Panels["tcurrentDirectory"].Text =
                                    Resources.HauptForm_DisplayRecordNumbers_Fügemodul_Datensatz____ + this.rowPos
                                    + Resources.HauptForm_DisplayRecordNumbers__von__ + this.bindManagerFuegeModul.Count;
                            }

                            if (this.bindManagerFuegeModul.Count > 0)
                            {
                                this.sliderFuegeModul.Maximum = this.bindManagerFuegeModul.Count - 1;
                                    // Maximalwert des Sliders festlegen
                                this.ultraNumericEditorNavigation.MaxValue = this.sliderFuegeModul.Maximum;
                                    // Maximalwert des Editors festlegen
                            }
                        }

                        // Slider auf ausgewählte Position stellen
                        // Wenn kein Datensatz vorhanden ist, Slider und Editor ausblenden
                        this.sliderFlexTool.Visible = false;
                        this.ultraNumericEditorNavigation.Visible = false;
                        if (rowIndex > 0)
                        {
                            // Falls der Index höher ist als die Anzahl Zeilen (kommt nach dem Löschen vor),
                            // auf den Maximalwert setzen
                            if (rowIndex > sliderFuegeModul.Maximum)
                            {
                                rowIndex = sliderFuegeModul.Maximum;
                            }

                            this.sliderFuegeModul.Value = rowIndex;
                            this.sliderFuegeModul.Visible = true;
                            this.ultraNumericEditorNavigation.Value = rowIndex;
                            this.ultraNumericEditorNavigation.Visible = true;
                        }
                        else
                        {
                            // Abfragen, ob Position im Bindmanager verändert werden darf
                            if (reglerEinstellen)
                            {
                                this.sliderFuegeModul.Value = this.sliderFuegeModul.Minimum;
                                this.sliderFuegeModul.Visible = false;
                                this.ultraNumericEditorNavigation.Value = this.sliderFuegeModul.Minimum;
                                this.ultraNumericEditorNavigation.Visible = false;
                            }
                        }
                        break;
                    }

                case (ushort)Werkzeug.FlexToolE12:
                    {
                        // Werkzeug ist ein FlexTool E12-Werkzeug, daher auswählen, ob Geber- oder Motorchip bearbeitet werden soll
                        if (geberEingabe)
                        {
                            // Es handelt sich um Geberdaten
                            if (bindManagerGeberE12 != null)
                            {
                                // Geberdaten
                                if (rowIndex >= 0)
                                {
                                    try
                                    {
                                        // Datensatzinfo in der Statuszeile anzeigen
                                        if (!LeeresWerkZeug)
                                        {
                                            statusBar.Panels["tcurrentDirectory"].Text =
                                                Resources.HauptForm_DisplayRecordNumbers_Geberchip_Datensatz____
                                                + (this.rowIndex + 1) + Resources.HauptForm_DisplayRecordNumbers__von__
                                                + this.bindManagerGeberE12.Count
                                                + Resources.HauptForm_DisplayRecordNumbers_
                                                + this.dataSetFlexToolGeberDatenE12.Tables["Geberchip"].Rows[rowIndex][
                                                    "Seriennummer"];
                                        }
                                        else
                                        {
                                            statusBar.Panels["tcurrentDirectory"].Text =
                                                Resources.HauptForm_DisplayRecordNumbers_Geberchip_Datensatz____
                                                + (this.rowIndex + 1) + Resources.HauptForm_DisplayRecordNumbers__von__
                                                + this.bindManagerGeberE12.Count
                                                + Resources.HauptForm_DisplayRecordNumbers_;
                                        }
                                    }
                                    catch
                                    {
                                        reglerEinstellen = false; // Position im Bindmanager nicht verstellen
                                        statusBar.Panels["tcurrentDirectory"].Text =
                                            Resources.HauptForm_DisplayRecordNumbers_Geberchip_Datensatz____
                                            + this.rowPos + Resources.HauptForm_DisplayRecordNumbers__von__
                                            + this.bindManagerGeberE12.Count;
                                    }
                                }
                                else
                                {
                                    reglerEinstellen = false; // Position im Bindmanager nicht verstellen
                                    statusBar.Panels["tcurrentDirectory"].Text =
                                        Resources.HauptForm_DisplayRecordNumbers_Geberchip_Datensatz____ + this.rowPos
                                        + Resources.HauptForm_DisplayRecordNumbers__von__
                                        + this.bindManagerGeberE12.Count;
                                }

                                if (this.bindManagerGeberE12.Count > 0)
                                {
                                    this.sliderFlexToolE12.Maximum = this.bindManagerGeberE12.Count - 1;
                                        // Maximalwert des Sliders festlegen
                                    this.ultraNumericEditorNavigation.MaxValue = this.sliderFlexToolE12.Maximum;
                                        // Maximalwert des Editors festlegen
                                }
                            }
                        }
                        else
                        {
                            // Es handelt sich um Motordaten
                            if (bindManagerMotorE12 != null)
                            {
                                // Motordaten
                                if (rowIndex >= 0)
                                {
                                    try
                                    {
                                        // Datensatzinfo in der Statuszeile anzeigen
                                        if (!LeeresWerkZeug)
                                        {
                                            statusBar.Panels["tcurrentDirectory"].Text =
                                                Resources.HauptForm_DisplayRecordNumbers_Motorchip_Datensatz____
                                                + (this.rowIndex + 1) + Resources.HauptForm_DisplayRecordNumbers__von__
                                                + this.bindManagerMotorE12.Count
                                                + Resources.HauptForm_DisplayRecordNumbers_
                                                + this.dataSetFlexToolMotorDatenE12.Tables["Motorchip"].Rows[rowIndex][
                                                    "Seriennummer Spindel"];
                                        }
                                        else
                                        {
                                            statusBar.Panels["tcurrentDirectory"].Text =
                                                Resources.HauptForm_DisplayRecordNumbers_Motorchip_Datensatz____
                                                + (this.rowIndex + 1) + Resources.HauptForm_DisplayRecordNumbers__von__
                                                + this.bindManagerMotorE12.Count
                                                + Resources.HauptForm_DisplayRecordNumbers_;
                                        }
                                    }
                                    catch
                                    {
                                        reglerEinstellen = false; // Position im Bindmanager nicht verstellen
                                        statusBar.Panels["tcurrentDirectory"].Text =
                                            Resources.HauptForm_DisplayRecordNumbers_Motorchip_Datensatz____
                                            + this.rowPos + Resources.HauptForm_DisplayRecordNumbers__von__
                                            + this.bindManagerMotorE12.Count;
                                    }
                                }
                                else
                                {
                                    reglerEinstellen = false; // Position im Bindmanager nicht verstellen
                                    statusBar.Panels["tcurrentDirectory"].Text =
                                        Resources.HauptForm_DisplayRecordNumbers_Motorchip_Datensatz____ + this.rowPos
                                        + Resources.HauptForm_DisplayRecordNumbers__von__
                                        + this.bindManagerMotorE12.Count;
                                }

                                if (bindManagerMotorE12.Count > 0)
                                {
                                    sliderFlexToolE12.Maximum = bindManagerMotorE12.Count - 1;
                                        // Maximalwert des Sliders festlegen
                                }
                            }
                        }

                        // Slider auf ausgewählte Position stellen
                        // Wenn kein Datensatz vorhanden ist, Slider und Editor ausblenden
                        ////this.sliderFlexToolE12.Visible = false;
                        ////this.ultraNumericEditorNavigation.Visible = false;
                        // Wenn kein Datensatz vorhanden ist, Slider ausblenden
                        if (rowIndex >= 0)
                        {
                            // Falls der Index höher ist als die Anzahl Zeilen (kommt nach dem Löschen vor),
                            // auf den Maximalwert setzen
                            if (rowIndex > this.sliderFlexToolE12.Maximum)
                            {
                                rowIndex = this.sliderFlexToolE12.Maximum;
                            }

                            this.sliderFlexToolE12.Value = rowIndex;
                            this.sliderFlexToolE12.Visible = true;
                            this.ultraNumericEditorNavigation.Value = rowIndex;
                            this.ultraNumericEditorNavigation.Visible = true;
                        }
                        else
                        {
                            // Abfragen, ob Position im Bindmanager verändert werden darf
                            if (reglerEinstellen)
                            {
                                // Position im Bindmanager darf verstellt werden
                                this.sliderFlexToolE12.Value = this.sliderFlexToolE12.Minimum;
                                this.sliderFlexToolE12.Visible = false;
                                this.ultraNumericEditorNavigation.Value = this.sliderFlexToolE12.Minimum;
                                this.ultraNumericEditorNavigation.Visible = false;
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
            UltraGrid grid = this.ultraGridErnaehrung; // Momentan aktives Grid 
            var bindingManager = this.bindingManagerGewicht; // Verwaltet die Datenanbindung
            var tabelle = string.Empty; // Name der Tabelle
            string table = $"{tabelle}"; // zum Zusammensetzen des Tabellen-Namens
            DataSet ds = this.dataSetGewicht1; // DataSet mit welchem gearbveitet wird

            // Überprüfen,Tabelle angewählt ist zum Einstellen der nötigen Variablen
            switch (this.selectedTab)
            {
                case (int)Ernaehrung:
                    {
                        grid = this.ultraGridErnaehrung; // Grid auswählen
                        bindingManager = this.bindingManagerGewicht; // Verwaltet die Gewichts-Daten
                        tabelle = "Gewicht"; // Name der Tabelle 
                        ds = this.dataSetGewicht1; // Zugehöriges DataSet
                        break;
                    }

                case (int)BlutDruck:
                    {
                        grid = this.ultraGridBlutDruck; // Grid auswählen
                        bindingManager = this.bindingManagerBlutDruck; // Verwaltet die Blutdruck-Daten 
                        tabelle = "BlutdruckDaten"; // Name der Tabelle 
                        ds = this.dataSetGewicht1; // Zugehöriges DataSet
                        break;
                    }
            }

            if (bindingManager.Position < ds.Tables[tabelle].Rows.Count && bindingManager.Position >= 0)
            {
                var aktIndex = grid.ActiveRow.Index; // Index der aktiven Zeile im Grid ermitteln
                this.indexNummerAktiveZeile = grid.Rows[aktIndex].Cells[0].Value.ToString();
                    // Zugehöriger Wert der Spalte 'Index' im Datensatz

                // Index im Bindmanager suchen, dazu alle Zeilen durchgehen
                for (var pos = 0; pos < bindingManager.Count; pos++)
                {
                    bindingManager.Position = pos; // Auf eine Zeile im Bindmanager positionieren

                    // Überprüfen, ob es sich um eine gelöschte Zeile handelt. Auf gelöschte Zeilen kann nicht zugegriffen werden
                    if (ds.Tables[tabelle].Rows[pos].RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    var indexTest = Convert.ToInt32(ds.Tables[tabelle].Rows[bindingManager.Position][0]);
                        // Index des Datensatzes ermitteln

                    // Indizes vergleichen. Ist der BindingManager inaktiv, ist die Position negativ
                    if (indexTest != Convert.ToInt32(this.indexNummerAktiveZeile) || bindingManager.Position < 0)
                    {
                        continue; // Index nicht gefunden, also weitersuchen
                    }

                    // Index des gesuchten Datensatzes gefunden, Position im Bindingmanager für Übertragung merken
                    this.rowIndex = bindingManager.Position;
                    this.indexNummer = ds.Tables[tabelle].Rows[bindingManager.Position][0].ToString();
                }
            }

            this.DisplayRecordNumbers();
        }

        #endregion DatasetAction
    }
}