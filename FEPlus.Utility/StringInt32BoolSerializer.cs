using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEPlus.Utility
{
    public class StringInt32BoolSerializer : BsonBaseSerializer
    {
        public override object Deserialize(BsonReader bsonReader, Type nominalType,
        Type actualType, IBsonSerializationOptions options)
        {
            var bsonType = bsonReader.CurrentBsonType;
            switch (bsonType)
            {
                case BsonType.Null:
                    bsonReader.ReadNull();
                    return string.Empty;
                case BsonType.String:
                    return bsonReader.ReadString();
                case BsonType.Int32:
                    return bsonReader.ReadInt32().ToString(CultureInfo.InvariantCulture);
                case BsonType.Boolean:
                    return bsonReader.ReadBoolean().ToString(CultureInfo.InvariantCulture);
                case BsonType.Double:
                    return bsonReader.ReadDouble().ToString();

                default:
                    var message = string.Format("Cannot deserialize BsonString,BsonBoolean or BsonInt32 from BsonType {0}.", bsonType);
                    throw new BsonSerializationException(message);
            }
        }

        public override void Serialize(BsonWriter bsonWriter, Type nominalType,
            object value, IBsonSerializationOptions options)
        {
            if (value != null)
            {
                bsonWriter.WriteString(value.ToString());
            }
            else
            {
                bsonWriter.WriteNull();
            }
        }
    }
}
