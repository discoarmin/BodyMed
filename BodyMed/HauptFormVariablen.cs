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
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Windows.Forms;

    using ComTools2.Hilfe;

    using DatenbankVergleich;

    using Datensicherung;
    using Kommunikation;

    using MeldungWerkzeug;

    using Infragistics.Win.UltraWinGrid;

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

        /// <summary>Position der Zeile im Blutdatensatz merken</summary>
        private int rowPosMerkBulDruck;

        /// <summary>Position der Zeile im Datensatz </summary>
        private string indexNummer;

        /// <summary>Indexnummer der aktiven Zeile </summary>
        private string indexNummerAktiveZeile;

        /// <summary>Index der aktiven Zeile </summary>
        private int rowIndex;

        /// <summary>Indexliste mit den vorhandenen flexTool-Werkzeugen</summary>
        private ArrayList indexListFlexToolE05;

        /// <summary>Information über die Kultur zur korrekten Datumsanzeige </summary>
        private System.Globalization.CultureInfo culture;

        /// <summary>Verwaltet die Datenanbindung der Gewichtsdaten</summary>
        private BindingManagerBase bindingManagerGewicht;

        /// <summary>Verwaltet die Datenanbindung der Blutdruckdaten</summary>
        private BindingManagerBase bindingManagerBlutDruck;

        /// <summary>Verwaltet die Datenanbindung der Blutdruckdaten</summary>
        private int selectedTab;

        #endregion Membervariablen

    }
}