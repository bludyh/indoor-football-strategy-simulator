using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Controls;
using IndoorFootballStrategySimulator.Simulation;

namespace IndoorFootballStrategySimulator {
    static class Utilities {

        public static Random Random { get; private set; }
        public static Texture2D SimpleTexture { get; set; }

        static Utilities() {
            Random = new Random();
        }

        public static void Serialize(object obj, string fileName) {
            var serializer = new DataContractSerializer(obj.GetType());
            var settings = new XmlWriterSettings { Indent = true };
            using (var writer = XmlWriter.Create(fileName, settings)) {
                serializer.WriteObject(writer, obj);
            }
        }

        public static T Deserialize<T>(string fileName) {
            var serializer = new DataContractSerializer(typeof(T));
            using (var fs = new FileStream(fileName, FileMode.Open))
            using (var reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas())) {
                return (T)serializer.ReadObject(reader, true);
            }
        }

    }
}
