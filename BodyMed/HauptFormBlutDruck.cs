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
//                  16.06.16  br      Grundversion
//      </para>
// </remarks>
// --------------------------------------------------------------------------------------------------------------------
namespace BodyMed
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    using Infragistics.Win.UltraWinGrid;

    using Resources = Properties.Resources;

    /// <summary>
    /// Ereignisse für Blutdruckdaten.
    /// </summary>
    [SuppressMessage("ReSharper", "RedundantEmptyDefaultSwitchBranch")]
    [SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases")]
    public partial class HauptForm : RibbonForm
    {
        /// <summary>
        /// Behandelt das 'AfterExitEditMode' Ereignis des ultraGridErnaehrung Controls.
        /// </summary>
        /// Der Editiermodus im ultraGridMotor wurde beendet
        private void OnUltraGridErnaehrungAfterExitEditMode(object sender, EventArgs e)
        {
            this.AfterExitEditMode(ref this.ultraGridErnaehrung, "Gewicht"); // Änderungen in Datenbank schreiben
        }

        /// <summary>Behandelt das AfterRowInsert Ereignis des ultraGridErnaehrung Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridErnaehrungAfterRowInsert(object sender, RowEventArgs e)
        {

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

        /// <summary>Behandelt das BeforeCellActivate Ereignis des ultraGridErnaehrung Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridErnaehrungBeforeCellActivate(object sender, CancelableCellEventArgs e)
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
        private void OUltraGridErnaehrungInitializePrint(object sender, CancelablePrintEventArgs e)
        {

        }

        /// <summary>Behandelt das Leave Ereignis des ultraGridErnaehrung Controls.</summary>
        /// <param name="sender">Die Quelle des Ereignisses.</param>
        /// <param name="e">Die <see cref="RowEventArgs"/> Instanz, welche die Ereignisdaten enthält.</param>
        private void OnUltraGridErnaehrungLeave(object sender, EventArgs e)
        {

        }
    }
}