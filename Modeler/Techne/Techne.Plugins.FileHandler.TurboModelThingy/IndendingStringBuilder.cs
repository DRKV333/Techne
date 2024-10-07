/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Plugins.FileHandler.TurboModelThingy
{
    class IndendingStringBuilder
    {
        private StringBuilder sb;
        private string indentationString = "\t";
        private string completeIndentationString = "";
        private int indent = 0;
        private bool newLine = true;
 
        /// <summary>
        ///  Creates an IndentedStringBuilder
        /// </summary>
        public IndendingStringBuilder()
        {
            sb = new StringBuilder();
        }
 
        /// <summary>
        /// Appends a string
        /// </summary>
        /// <param name="value"></param>
        public void Append(string value)
        {
            if (newLine)
            {
                sb.Append(completeIndentationString);
                newLine = false;
            }

            sb.Append(value);
        }

        public void AppendLine()
        {
            AppendLine("");
        }
        /// <summary>
        /// Appends a line
        /// </summary>
        /// <param name="value"></param>
        public void AppendLine(string value)
        {
            Append(value + Environment.NewLine);
            newLine = true;
        }
 
        /// <summary>
        /// The string/chars to use for indentation, \t by default
        /// </summary>
        public string IndentationString
        {
            get { return indentationString; }
            set
            {
                indentationString = value;
 
                updateCompleteIndentationString();
            }
        }
 
        /// <summary>
        /// Creates the actual indentation string
        /// </summary>
        private void updateCompleteIndentationString()
        {
            completeIndentationString = "";
 
            for (int i = 0; i < indent; i++)
                completeIndentationString += indentationString;
        }
 
        /// <summary>
        /// Increases indentation, returns a reference to an IndentedStringBuilder instance which is only to be used for disposal
        /// </summary>
        /// <returns></returns>
        public void IncreaseIndent()
        {
            indent++;
 
            updateCompleteIndentationString();
        }

        /// <summary>
        /// Decreases indentation, may only be called if indentation > 1
        /// </summary>
        public void DecreaseIndent()
        {
            if (indent > 0)
            {
                indent--;

                updateCompleteIndentationString();
            }
        }
 
        /// <summary>
        /// Returns the text of the internal StringBuilder
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return sb.ToString();
        }

        internal void AppendLines(IndendingStringBuilder fieldDefinitions)
        {
            foreach (var item in fieldDefinitions.ToString().Replace("\r\n","\n").Split('\n'))
            {
                if (!(String.IsNullOrEmpty(item) || String.IsNullOrWhiteSpace(item)))
                {
                    AppendLine(item);
                }
            }
        }
    }
}

