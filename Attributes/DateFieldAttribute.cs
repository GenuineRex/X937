﻿using System;
using System.IO;
using System.Reflection;

namespace X937.Attributes
{
    /// <summary>
    /// Defines a Date field on the property.
    /// </summary>
    public class DateFieldAttribute : FieldAttribute
    {
        /// <summary>
        /// Defines a new date field attribute on a record.
        /// </summary>
        /// <param name="fieldNumber">The order of this field on the record.</param>
        public DateFieldAttribute( int fieldNumber )
            : base( fieldNumber, 8 )
        {
        }

        /// <summary>
        /// Decode a field from a reader into the specified property.
        /// </summary>
        /// <param name="reader">The reader we will be reading from.</param>
        /// <param name="record">The object whose property will be set.</param>
        /// <param name="property">The information about the property to be set.</param>
        public override void DecodeField( BinaryReader reader, Record record, PropertyInfo property )
        {
            DateTime value;

            if ( DateTime.TryParseExact( reader.ReadEbcdicString( 8 ), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out value ) )
            {
                property.SetValue( record, value );
            }
            else
            {
                if ( property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof( Nullable<> ) )
                {
                    property.SetValue( record, null );
                }
                else
                {
                    throw new Exception( "Null date found in data but property not nullable." );
                }
            }
        }

        /// <summary>
        /// Encode a field by writing the property value into the writer.
        /// </summary>
        /// <param name="writer">The writer that will contain the encoded data.</param>
        /// <param name="record">The object whose property will be encoded.</param>
        /// <param name="property">The information abou the property to be encoded.</param>
        public override void EncodeField( BinaryWriter writer, Record record, PropertyInfo property )
        {
            var value = ( DateTime? ) property.GetValue( record );

            string valueStr = value.HasValue ? value.Value.ToString( "yyyyMMdd" ) : new string( ' ', Size );

            writer.WriteEbcdicString( valueStr );
        }
    }
}
