using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PokerTable.Game.Models;

namespace PokerTable.Web.ViewModels
{
    /// <summary>
    /// Table View View Model
    /// </summary>
    public class TableViewViewModel
    {
        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        public Table Table { get; set; }

        /// <summary>
        /// Gets or sets the table JSON.
        /// </summary>
        /// <value>
        /// The table JSON.
        /// </value>
        public string TableJSON { get; set; }
    }
}