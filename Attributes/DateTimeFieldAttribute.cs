﻿using System;
using System.IO;
using System.Reflection;

namespace X937.Attributes
{
    /// <summary>
    /// Defines a Date and Time field on the property.
    /// </summary>
    public class DateTimeFieldAttribute : FieldAttribute
    {
        /// <summary>
        /// Defines a new date and time field attribute on a record.
        /// </summary>
        /// <param name="fieldNumber">The order of this field on the record.</param>
        public DateTimeFieldAttribute( int fieldNumber )
            : base( fieldNumber, 12 )
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
            var value = DateTime.ParseExact( reader.ReadEbcdicString( 12 ), "yyyyMMddHHmm", null );

            property.SetValue( record, value );
        }

        /// <summary>
        /// Encode a field by writing the property value into the writer.
        /// </summary>
        /// <param name="writer">The writer that will contain the encoded data.</param>
        /// <param name="record">The object whose property will be encoded.</param>
        /// <param name="property">The information abou the property to be encoded.</param>
        public override void EncodeField( BinaryWriter writer, Record record, PropertyInfo property )
        {
            DateTime value = ( DateTime ) property.GetValue( record );

            writer.WriteEbcdicString( value.ToString( "yyyyMMddHHmm" ) );
        }
    }
}