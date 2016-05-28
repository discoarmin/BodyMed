namespace BodyMed
{
    using System;
    using System.Windows.Forms;
    //using ZedGraph;

    public partial class HauptForm : RibbonForm
    {
        #region Variablen

        /// <summary>Verwaltet die Datenanbindung der Gewichtsdaten</summary>
        private BindingManagerBase bindingManagerGewicht;

        /// <summary>Verwaltet die Datenanbindung der Blutdruckdaten</summary>
        private BindingManagerBase bindingManagerBlutDruck;

        /// <summary>Verwaltet die Datenanbindung der Blutdruckdaten</summary>
        private int selectedTab;
        #endregion

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
                case "Eingabe":                                                 // Ernährungsdaten eingaben
                    this.SetzeAufErnaehrung();
                    break;
                case "Blutdruck":                                               // Blutdruckdaten eingaben
                    this.SetzeAufBlutDruck();
                    break;
                case "Neu":                                                     // Neuer Datensatz
                    // Zuerst ermitteln, welche Eingabe aktiv ist
                    switch (this.selectedTab)
                    {
                        case 1:                                                 // Gewichtseingabe
                            this.ultraGridErnaehrung.Rows.Band.AddNew();        // Neuen Datensatz hizufügen
                            break;
                        case 2:                                                 // Eingabe Blutdruckdaten
                            this.ultraGridBlutDruck.Rows.Band.AddNew();         // Neuen Datensatz hizufügen
                            break;
                    }

                    break;
            }
        }

        /// <summary>Zeigt die Daten für die Gewichtseingabe an.</summary>
        private void SetzeAufErnaehrung()
        {
            this.ultraTabControlHauptForm.Tabs["Ernaehrung"].Selected = true;   // Eingabe der Gewichtsdaten
            this.selectedTab = this.ultraTabControlHauptForm.SelectedTab.Index; // Nummer des ausgewählten Tabs merken
        }

        /// <summary>Zeigt die Daten für die Blutdruckeingabe an.</summary>
        private void SetzeAufBlutDruck()
        {
            this.ultraTabControlHauptForm.Tabs["BlutDruck"].Selected = true;    // Eingabe der Blutdruckdaten
            this.selectedTab = this.ultraTabControlHauptForm.SelectedTab.Index; // Nummer des ausgewählten Tabs merken
        }

        /// <summary> Manager für Datenbankanbindungen zu den einzelnen Tabellen der Datenbank bereitstellen. </summary>
        private void LadeDatenBank()
        {
            this.bindingManagerGewicht = this.BindingContext[this.dataSetGewicht1.Tables["Gewicht"]];
            this.bindingManagerBlutDruck = this.BindingContext[this.dataSetErnaehrung1.Tables["BlutdruckDaten"]];
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
    }
}