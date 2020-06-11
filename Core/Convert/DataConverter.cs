using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Microline.WS.Core.Convert
{
    public class DataConverter
    {


        public enum Action { Return0, ReturnNull, ReturnDefault, ThrowError, ThrowWarning, EmptyNullElseWarning };


        /// <summary>
        /// Converts to boolean
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBoolean(object value)
        {
            try
            {
                if (value == null)
                    return false;
                else
                    return System.Convert.ToBoolean(value);
            }
            catch (Exception ex) { throw new InvalidCastException("Object cannot be converted to boolean", ex); }
        }

        /// <summary>
        /// Returns non nullable int value, if invalid of null returns 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt32(object value)
        {
            return ToInt32(value, Action.Return0, 0) ?? 0;
        }

        /// <summary>
        /// Return nullable int 32 value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="whatIfNotValid"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int? ToInt32(object value, Action whatIfNotValid, int defaultValue)
        {
            if (value is int?)
                return (int?)value;
            else
            {
                if (value is double?)
                {
                    double? objCast = (double?)value;
                    if (objCast == 0)
                        throw new Exception("Conversion of null to Int32 not possible");
                    else
                        return new int?(System.Convert.ToInt32(objCast ?? 0));
                }
                else
                {
                    if (value == null)
                        return null;
                    else
                        try
                        {
                            if (value is string)
                            {
                                if (value.ToString().TrimEnd() == "")
                                    return invalidInt32("", whatIfNotValid, defaultValue);
                                else
                                    return (int?)int.Parse(value.ToString());
                            }
                            else
                                return new int?(System.Convert.ToInt32(value));
                        }
                        catch (Exception) { return invalidInt32(value, whatIfNotValid, defaultValue); }
                }
            }
        }



        private static int? invalidInt32(object value, Action whatIfNotValid, int defaultValue)
        {
            try
            {
                switch (whatIfNotValid)
                {
                    case Action.Return0:
                        return 0;
                    case Action.ReturnDefault:
                        return defaultValue;
                    case Action.ReturnNull:
                        return null;
                    case Action.EmptyNullElseWarning:
                        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                            return null;
                        else
                            throw new InvalidValueException(String.Format("{0} nije cijeli broj", value));
                    default:
                    case Action.ThrowError:
                        throw new InvalidValueException(String.Format("{0} is not a proper integer", value));
                    case Action.ThrowWarning:
                        throw new InvalidValueException(String.Format("{0} nije cijeli broj", value));
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public static string FormatAsXML(string xml)
        {
            string result = "";

            MemoryStream mStream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.Unicode);
            XmlDocument document = new XmlDocument();

            try
            {
                // Load the XmlDocument with the XML.
                document.LoadXml(xml);

                writer.Formatting = Formatting.Indented;

                // Write the XML into a formatting XmlTextWriter
                document.WriteContentTo(writer);
                writer.Flush();
                mStream.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                mStream.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                StreamReader sReader = new StreamReader(mStream);

                // Extract the text from the StreamReader.
                string formattedXml = sReader.ReadToEnd();

                result = formattedXml;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (mStream != null) mStream.Close();
                if (writer != null) writer.Close();
            }

            return result;
        }
    }
}
