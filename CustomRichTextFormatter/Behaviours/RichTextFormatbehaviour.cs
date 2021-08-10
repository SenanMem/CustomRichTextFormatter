using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace CustomRichTextFormatter.Behaviours
{
   public class RichTextFormatbehaviour:Behavior<FrameworkElement>
    {
        #region Field
        public UserControl ParentContentPane = null;
        public RichTextBox RichTextControl = null;
        public ComboBox FontFamilyType = null;
        public ComboBox FontHeight = null;
        public ToggleButton ToolStripButtonStrikeout = null;
        public ToggleButton ToolStripButtonAlignLeft = null;
        public ToggleButton ToolStripButtonAlignCenter = null;
        public ToggleButton ToolStripButtonAlignRight = null;
        public ToggleButton ToolStripButtonUnderline = null;
        public ToggleButton ToolStripButtonItalic = null;
        public ToggleButton ToolStripButtonBold = null;
        #endregion

        #region Properties

        private int _BlankLine = 1;
        public int BlankLine
        {
            get { return _BlankLine; }
            set
            {
                _BlankLine = value;
            }
        }

        private int _BlankColumn = 1;
        public int BlankColumn
        {
            get { return _BlankColumn; }
            set
            {
                _BlankColumn = value;
            }
        }
        #endregion

        
        protected override void OnAttached()
        {
            try
            {
                base.OnAttached();
                if (ParentContentPane == null)
                    ParentContentPane = (UserControl)AssociatedObject;
                if (ParentContentPane == null) return;
                //customRtfRichTextEdit = (UserControl)_parentContentPane.FindName("MainUserControl");
                RichTextControl = (RichTextBox)ParentContentPane.FindName("RichTextControl");
                RichTextControl.SelectionChanged += RichTextControl_SelectionChanged;
                RichTextControl.TextChanged += RichTextControl_TextChanged;
                RichTextControl.LostFocus += RichTextControl_LostFocus;
                RichTextControl.KeyDown += RichTextControl_KeyDown;
                RichTextControl.KeyUp += RichTextControl_KeyUp;
                ToolStripButtonUnderline = (ToggleButton)ParentContentPane.FindName("ToolStripButtonUnderline");
                ToolStripButtonItalic = (ToggleButton)ParentContentPane.FindName("ToolStripButtonItalic");
                ToolStripButtonBold = (ToggleButton)ParentContentPane.FindName("ToolStripButtonBold");
                FontFamilyType = (ComboBox)ParentContentPane.FindName("Fonttype");
                FontHeight = (ComboBox)ParentContentPane.FindName("Fontheight");
                ToolStripButtonStrikeout = (ToggleButton)ParentContentPane.FindName("ToolStripButtonStrikeout");
                ToolStripButtonAlignLeft = (ToggleButton)ParentContentPane.FindName("ToolStripButtonAlignLeft");
                ToolStripButtonAlignCenter = (ToggleButton)ParentContentPane.FindName("ToolStripButtonAlignCenter");
                ToolStripButtonAlignCenter.Click += ToolStripButtonAlignCenter_Click;
                ToolStripButtonAlignRight = (ToggleButton)ParentContentPane.FindName("ToolStripButtonAlignRight");
                ToolStripButtonAlignRight.Click += ToolStripButtonAlignRight_Click;
                ToolStripButtonAlignLeft.Click += ToolStripButtonAlignLeft_Click;
                RichTextControl.Loaded += RichTextControl_Loaded;
                ToolStripButtonStrikeout.Click += ToolStripButtonStrikeout_Click;
                FontFamilyType.DropDownClosed += FontFamilyType_DropDownClosed;
                FontHeight.DropDownClosed += FontHeight_DropDownClosed;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RichTextControl_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //Using Ctrl + B
            if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.B))
            {
                ToolStripButtonBold.IsChecked = ToolStripButtonBold.IsChecked != true;
            }

            //Using Ctrl + I
            if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.I))
            {
                ToolStripButtonItalic.IsChecked = ToolStripButtonItalic.IsChecked != true;
            }

            //Using Ctrl + U
            if ((Keyboard.Modifiers != ModifierKeys.Control) || (e.Key != Key.U)) return;
            ToolStripButtonUnderline.IsChecked = ToolStripButtonUnderline.IsChecked != true;
        }

        private void RichTextControl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var fontNameValue = (string)FontFamilyType.SelectedValue;
            var fontHeightValue = (string)FontHeight.SelectedValue;
            var range = new TextRange(RichTextControl.Selection.Start, RichTextControl.Selection.End);
            range.ApplyPropertyValue(TextElement.FontFamilyProperty, fontNameValue);
            range.ApplyPropertyValue(TextElement.FontSizeProperty, fontHeightValue);
        }

        private void RichTextControl_TextChanged(object sender, TextChangedEventArgs e)
        {
            var range = new TextRange(RichTextControl.Document.ContentStart, RichTextControl.Document.ContentEnd);
            ParentContentPane.Uid = Regex.Replace(range.Text, @"\t|\n|\r", "");
            ParentContentPane.Tag = range.Text;
        }

        private void RichTextControl_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectionRange = new TextRange(RichTextControl.Selection.Start, RichTextControl.Selection.End);
                if (selectionRange.GetPropertyValue(FlowDocument.TextAlignmentProperty).ToString() == "Left")
                {
                    ToolStripButtonAlignLeft.IsChecked = true;
                }
                if (selectionRange.GetPropertyValue(FlowDocument.TextAlignmentProperty).ToString() == "Center")
                {
                    ToolStripButtonAlignCenter.IsChecked = true;
                }
                if (selectionRange.GetPropertyValue(FlowDocument.TextAlignmentProperty).ToString() == "Right")
                {
                    ToolStripButtonAlignRight.IsChecked = true;
                }
                FontFamilyType.SelectedValue = selectionRange.GetPropertyValue(FlowDocument.FontFamilyProperty).ToString();
                FontHeight.SelectedValue = selectionRange.GetPropertyValue(FlowDocument.FontSizeProperty).ToString();
                BlankLine = TextPointerPosition();
                BlankColumn = ColumnNumber();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ToolStripButtonAlignRight_Click(object sender, RoutedEventArgs e)
        {
            if (ToolStripButtonAlignRight.IsChecked != true) return;
            ToolStripButtonAlignCenter.IsChecked = false;
            ToolStripButtonAlignLeft.IsChecked = false;
        }

        private void ToolStripButtonAlignCenter_Click(object sender, RoutedEventArgs e)
        {
            if (ToolStripButtonAlignCenter.IsChecked != true) return;
            ToolStripButtonAlignLeft.IsChecked = false;
            ToolStripButtonAlignRight.IsChecked = false;
        }

        private void FontHeight_DropDownClosed(object sender, EventArgs e)
        {
            var fontHeightValue = (string)FontHeight.SelectedItem;
            if (fontHeightValue == null) return;
            RichTextControl.Selection.ApplyPropertyValue(Control.FontSizeProperty, fontHeightValue);
            RichTextControl.Focus();
        }

        private void FontFamilyType_DropDownClosed(object sender, EventArgs e)
        {
            var fontNameValue = (string)FontFamilyType.SelectedItem;
            if (fontNameValue == null) return;
            RichTextControl.Selection.ApplyPropertyValue(Control.FontFamilyProperty, fontNameValue);
        }

        private void ToolStripButtonStrikeout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var range = new TextRange(RichTextControl.Selection.Start, RichTextControl.Selection.End);
                var tdc = (TextDecorationCollection)RichTextControl.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
                if (tdc == null || !tdc.Equals(TextDecorations.Strikethrough))
                {
                    tdc = TextDecorations.Strikethrough;

                }
                else
                {
                    tdc = new TextDecorationCollection();
                }
                range.ApplyPropertyValue(Inline.TextDecorationsProperty, tdc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RichTextControl_LostFocus(object sender, RoutedEventArgs e)
        {
            var range = new TextRange(RichTextControl.Document.ContentStart, RichTextControl.Document.ContentEnd);
            ParentContentPane.Uid = Regex.Replace(range.Text, @"\t|\n|\r", "");
            ParentContentPane.Tag = range.Text;
        }

        private void RichTextControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
               
                var range = new TextRange(RichTextControl.Selection.Start, RichTextControl.Selection.End);
                FontFamilyType.SelectedValue = range.GetPropertyValue(FlowDocument.FontFamilyProperty).ToString();
                FontHeight.SelectedValue = range.GetPropertyValue(FlowDocument.FontSizeProperty).ToString();
                BlankLine = TextPointerPosition();
                BlankColumn = ColumnNumber();
                RichTextControl.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ToolStripButtonAlignLeft_Click(object sender, RoutedEventArgs e)
        {
            if (ToolStripButtonAlignLeft.IsChecked != true) return;
            ToolStripButtonAlignCenter.IsChecked = false;
            ToolStripButtonAlignRight.IsChecked = false;
        }

        #region All Methods
        private int TextPointerPosition()
        {
            var currentLineNumber = 1;
            try
            {
                var caretLineStart = RichTextControl.CaretPosition.GetLineStartPosition(0);
                var p = RichTextControl.Document.ContentStart.GetLineStartPosition(0);
                while (true)
                {
                    if (caretLineStart.CompareTo(p) < 0)
                    {
                        break;
                    }
                    p = p.GetLineStartPosition(1, out int result);
                    if (result == 0)
                    {
                        break;
                    }
                    currentLineNumber++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return currentLineNumber;
        }

        private int ColumnNumber()
        {
            var currentColumnNumber = 0;
            try
            {
                var caretPos = RichTextControl.CaretPosition;
                var p = RichTextControl.CaretPosition.GetLineStartPosition(0);
                currentColumnNumber = Math.Max(p.GetOffsetToPosition(caretPos) - 1, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MethodBase.GetCurrentMethod().Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            return currentColumnNumber;
        }
        #endregion
    }
}
