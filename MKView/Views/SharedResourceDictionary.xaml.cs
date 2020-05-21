using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MKView.Views
{
    /// <summary>
    /// Interaction logic for SharedResresourceDictionary.xaml
    /// </summary>
    public partial class SharedResourceDictionary 
    {
        /// <summary>
        /// The static instance for Shared Resource Dictionary.
        /// </summary>
        private static readonly SharedResourceDictionary instance = new SharedResourceDictionary();

        /// <summary>
        /// Initializes a new instance of the <see cref="SharedResourceDictionary" /> class.
        /// </summary>
        public SharedResourceDictionary()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static SharedResourceDictionary Instance => instance;
    }
}
