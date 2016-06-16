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
namespace BodyMed
{
    using System.Collections;
    using System.Windows.Forms;

    /// <summary>
    /// Die Variablendeklaration.
    /// </summary>
    public partial class HauptForm
    {
        #region Membervariablen

        /// <summary>Position der Zeile im Datensatz </summary>
        private int rowPos;

        /// <summary>Position der Zeile im Datensatz merken</summary>
        private int rowPosMerk;

        /// <summary>Position der Zeile im Ernährungsdatensatz merken</summary>
        private int rowPosMerkErnaehrung;

        /// <summary>Position der Zeile im Blutdruckdatensatz merken</summary>
        private int rowPosMerkBlutDruck;

        /// <summary>Position der Zeile im Datensatz </summary>
        private string indexNummer;

        /// <summary>Indexnummer der aktiven Zeile </summary>
        private string indexNummerAktiveZeile;

        /// <summary>Index der aktiven Zeile </summary>
        private int rowIndex;

        /// <summary>Indexliste mit den ausgewählten Zeilen im ultraGridErnaehrung</summary>
        private ArrayList indexListErnaehrung;

        /// <summary>Information über die Kultur zur korrekten Datumsanzeige </summary>
        private System.Globalization.CultureInfo culture;

        /// <summary>Verwaltet die Datenanbindung der Gewichtsdaten</summary>
        private BindingManagerBase bindingManagerGewicht;

        /// <summary>Verwaltet die Datenanbindung der Blutdruckdaten</summary>
        private BindingManagerBase bindingManagerBlutDruck;

        /// <summary>Verwaltet die Datenanbindung der Blutdruckdaten</summary>
        private int selectedTab;

        /// <summary>Merker, ob die Position bei den Ernährungsdaten geändert werden darf</summary>
        private bool gewichtEinstellen;

        /// <summary>Merker, ob die Position bei den Blutdruckdaten geändert werden darf</summary>
        private bool blutDruckEinstellen;

        /// <summary>Merker, ob Auswahlliste neu ermittelt werden darf</summary>
        private bool auswahllisteLoeschen;


        #endregion Membervariablen

        /// <summary>Aufzählung für Formular</summary>
        public enum Formular
        {
            /// <summary>Ernährungs</summary>
            Ernaehrung = 0,

            /// <summary>Blutdruck'</summary>
            BlutDruck = 1
        }

    }
}