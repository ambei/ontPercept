/// <summary>
///
/// This file is part of OntCog project.
/// 
/// OntCog is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by
/// the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
/// 
/// OntCog is distributed in the hope that it will be useful,  but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU General Public License for more details.
/// 
/// 
/// You should have received a copy of the GNU General Public License  along with Foobar. 
/// If not, see<http://www.gnu.org/licenses/>.
/// 
///  This file defines sparql commands to be sent to the triple store.
/// Basically, the following XML Schema (XSD) formats are used:
///     xsd:dateTime - Instant of time(Gregorian calendar) Format:[-] CCYY-MM-DDThh:mm:ss[Z | (+| -)hh:mm]
///     xsd:double  - A signed (64 bits) floating-point number as defined by the IEEE (Institute of Electrical and Electronic Engineers)
///     xsd:long	- A signed 64-bit integer. Integers between -9223372036854775808 and 9223372036854775807.
///		      Note: 0000000000 has a special meaning for the API, so its not used as a identifier. 
///                   Note: only 32bits are used by the Unity tool. So, in this implementation we will consider 10 digits.
///                   Note: When used as OntSense identifier the first caracter defines the kind of identifier:
///                         "O" : defines an object.            Example: O0000000789
///                         "P" : defines a object position.    Example: P0000000123
///                         "L" : defines a local in the space. Example: L9876543210 
///                         "C" : defines a color.              Exemple: C0000000456
///                         "V" : vision event identifier       Example: V9876543211 
///                         "S" : smell event identifier        Example: S9876543212 
///                         "A" : taste event identifier        Example: A9876543213 
///                         "T" : touch event identifier        Example: T9876543214 
///                         "H" : hear event identifier         Example: H1961080801 
/// 
///                    Note: Object is the superclass of: Human and Robot concepts, so they also starts with "O" letter
/// 
/// 
/// 
///     xsd:string  - Represents a character string that may contain any Unicode character allowed by XML.
///     xsd:anyURI  - Represents a Uniform Resource Identifier (URI) reference. In this version all URI references are generated by www.wikidata.org
///                   Examples: Drinking vessel:drinking glass      -> https://www.wikidata.org/wiki/Q81727
///                             Cup: mug                            -> https://www.wikidata.org/wiki/Q386215                   
///                             Food::cherry pie                    -> https://www.wikidata.org/wiki/Q5092519
///                             Meat::salmon                        -> https://www.wikidata.org/wiki/Q7405484
///                             Meat::beef                          -> https://www.wikidata.org/wiki/Q192628
///                             Finger food: cracker                -> https://www.wikidata.org/wiki/Q856330
///                             Architectural element::floor        -> https://www.wikidata.org/wiki/Q217164
///                             Architectural element::door         -> https://www.wikidata.org/wiki/Q36794                             
///                             Forniture::table                    -> https://www.wikidata.org/wiki/Q14748
///                             Switch: light switch                -> https://www.wikidata.org/wiki/Q962420
///                             Device:tap                          -> https://www.wikidata.org/wiki/Q656656                            
///                             
///                     

/// 
/// To convert from the internal representation in C# to a sparql command we use the following C# format strings
///     xsd:dateTime - use C# string format => "yyyy-MM-ddThh:mm:ss.fff"        Note: supports until millisecond
///     xsd:double   - use C# string format => "G"         Note: The more compact of either fixed-point or scientific notation.
///     xsd:long	 - use C# string format => "D10"       Note: Only 10 digits will be considered in this version. Reason: it will be more than the necessary.
///     xsd:string   - default C# string format, just {<number>}
///     xsd:anyURI   - default C# string format, just {<number>}
///     
/// 
/// The basicPos.owl file is the only file changed relative to the original cora.zip developed at UFRGS. 
/// When I entered "color" I realized that the "Data property cartesianZ" was linked to the ontSense ontology, but cartesianX and cartesianY were 
/// associated with BasicPos.owl defined by the UFRGS group. I considered this structure ungraceful, so I inserted "Data property cartesianZ" directly 
/// into basicPos.owl. With this in a system recovery, do not just recover the package cora.zip! You also need to update the BasicPos.owl file.

/// 
/// </summary>

namespace OntSenseCSharpAPI
{
	public sealed class SparqlAccess
    {
        /// Define a string format adherent to XML Schema (XSD) Date and Time Data type.
        /// for use:  String = DateTime.ToString(XSD_DATETIME);
        public static readonly string XSD_DATETIME = @"yyyy-MM-ddTHH:mm:ss.fff";



        /// script for a sparql insert operation with a cartesian point information.
        /// This script must be used when we have a object associated with the position 
        public static readonly string INSERT_POSITION_OBJ =
    "PREFIX ontsense: <http://example.org/sense#>" +
    "PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>" +
    "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>" +
    "PREFIX owl: <http://www.w3.org/2002/07/owl#>" +
    "PREFIX basicPos: <http://www.inf.ufrgs.br/phi-group/ontologies/basicPos.owl#>" +
    "INSERT DATA" +
    "    {{" +
    "        ontsense:P{0:D10}  rdf:type basicPos:CartesianPositionPoint;" +         // {0} defines the  object position point name  Ex:  P0000000789
    "        rdf:type owl:NamedIndividual;" +
    "        basicPos:cartesianX 	\"{1:G}\"^^xsd:double ;" +                      // {1} defines the posX  Ex:  20.123
    "        basicPos:cartesianY 	\"{2:G}\"^^xsd:double ;" +                      // {2} defines the posY  Ex:  20.123
    "        basicPos:cartesianZ 	\"{3:G}\"^^xsd:double ." +                      // {3} defines the posY  Ex:  20.123
    "}}";

        /// script for a sparql insert operation with a cartesian point information.
        /// This script must be used when the position just represents a local in the Space cartesian coordenate and we do not know the associated Object
        public static readonly string INSERT_POSITION_LOCAL =
    "PREFIX ontsense: <http://example.org/sense#>" +
    "PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>" +
    "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>" +
    "PREFIX owl: <http://www.w3.org/2002/07/owl#>" +
    "PREFIX basicPos: <http://www.inf.ufrgs.br/phi-group/ontologies/basicPos.owl#>" +
    "INSERT DATA" +
    "    {{" +
    "        ontsense:L{0:D10}  rdf:type basicPos:CartesianPositionPoint;" +         // {0} defines the  local position point name  Ex:  L0000001961
    "        rdf:type owl:NamedIndividual;" +
    "        basicPos:cartesianX 	\"{1:G}\"^^xsd:double ;" +                      // {1} defines the posX  Ex:  20.123
    "        basicPos:cartesianY 	\"{2:G}\"^^xsd:double ;" +                      // {2} defines the posY  Ex:  20.123
    "        basicPos:cartesianZ 	\"{3:G}\"^^xsd:double ." +                      // {3} defines the posY  Ex:  20.123
    "}}";




        /// script for a sparql insert operation with color information.
        public static readonly string INSERT_COLOR = 
    "PREFIX ontsense: <http://example.org/sense#>" +
    "PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>" +
    "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>" +
    "PREFIX owl: <http://www.w3.org/2002/07/owl#>" +
    "INSERT DATA" +
    "    {{" +
    "        ontsense:C{0:D10}  rdf:type ontsense:RGBColor;" +         // {0} defines the  color  Ex:  C0000000789
    "        rdf:type           owl:NamedIndividual;" +
    "        ontsense:red 	    \"{1:G}\"^^xsd:double ;" +             // {1} defines the red component  Ex:    70.1E-10
    "        ontsense:green     \"{2:G}\"^^xsd:double ;" +             // {2} defines the green component  Ex:  71.1E-11
    "        ontsense:blue 	    \"{3:G}\"^^xsd:double ." +             // {3} defines the blue component  Ex:   72.1E-12
    "}}";



        /// script for a sparql insert operation with ordinary object information.
        public static readonly string INSERT_THING =
    "PREFIX sumo: <http://www.inf.ufrgs.br/phi-group/ontologies/sumo.owl#>" +
    "PREFIX ontsense: <http://example.org/sense#>" +
    "PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>" +
    "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>" +
    " INSERT DATA" +
    "    {{" +
    "            ontsense:O{0:D10} rdf:type sumo:Object;" +        // {0} defines the object Ex:  O0000000789
    "            ontsense:objectId 	\"{1}\"^^xsd:long ;" +         // {1} defines the unique objectId  Ex:  1234567892		 		
    "            ontsense:hasColor ontsense:C{0:D10} ;" +          // {0} also defines the color id Ex:  C0000000789		
    "            ontsense:isMadeOf ontsense:{2} ; " +              // {2} defines the Material		
    "            ontsense:isPositionedAt ontsense:P{0:D10} ;" +    // {0} also defines the CartesianPosition id Ex:  P0000000789	
    "            ontsense:hasInternalState ontsense:{3}; " +       // {3} defines the InternalState
    "            ontsense:tag        	\"{4}\"; " +               // {4} defines a tag associated with the concept
    "            ontsense:associateURI \"{5}\"; " +                // {5} defines the URI address
    "            ontsense:name        	\"{6}\". " +               // {6} defines a name associated with the concept
    "}}";



        /// script for a sparql insert operation with Human agent information
        public static string INSERT_HUMAN =
    "PREFIX sumo: <http://www.inf.ufrgs.br/phi-group/ontologies/sumo.owl#>" +
    "PREFIX ontsense: <http://example.org/sense#>" +
    "PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>" +
    "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>" +
    " INSERT DATA" +
    "    {{" +
    "            ontsense:O{0:D10} rdf:type ontsense:Human;" +     // {0} defines the object Ex:  O0000000789
    "            ontsense:objectId 	\"{1}\"^^xsd:long ;" +         // {1} defines the unique objectId  Ex:  1234567892		 		
    "            ontsense:hasColor ontsense:C{0:D10} ;" +          // {0} also defines the Color  Ex:  C0000000789		
    "            ontsense:isMadeOf ontsense:{2} ; " +              // {2} defines the Material		
    "            ontsense:isPositionedAt ontsense:P{0:D10} ;" +    // {0} also defines the CartesianPosition  Ex:  P0000000789	
    "            ontsense:hasInternalState ontsense:{3}; " +       // {3} defines the InternalState
    "            ontsense:tag        	\"{4}\"; " +               // {4} defines a tag associated with the concept
    "            ontsense:associateURI \"{5}\"; " +                // {5} defines the URI address
    "            ontsense:hasEmotionalState ontsense:{6}; " +      // {6} defines the Emotional State
    "            ontsense:name        	\"{7}\". " +               // {7} defines a name associated with the concept
    "}}";





        /// script for a sparql insert operation with Robot agent information
        public static string INSERT_ROBOT =
        "PREFIX sumo: <http://www.inf.ufrgs.br/phi-group/ontologies/sumo.owl#>" +
        "PREFIX ontsense: <http://example.org/sense#>" +
        "PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>" +
        "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>" +
	"PREFIX cora: <http://www.inf.ufrgs.br/phi-group/ontologies/cora.owl#> " +
        " INSERT DATA" +
        "    {{" +
        "            ontsense:O{0:D10} rdf:type cora:Robot;" +         // {0} defines the object Ex:  O0000000789
        "            ontsense:objectId 	\"{1}\"^^xsd:long ;" +         // {1} defines the unique objectId  Ex:  1234567892		 		
        "            ontsense:hasColor ontsense:C{0:D10} ;" +          // {0} also defines the Color  Ex:  C0000000789		
        "            ontsense:isMadeOf ontsense:{2} ; " +              // {2} defines the Material		
        "            ontsense:isPositionedAt ontsense:P{0:D10} ;" +    // {0} also defines the CartesianPosition  Ex:  P0000000789	
        "            ontsense:hasEmotionalState ontsense:{3}; " +      // {3} defines the InternalState
        "            ontsense:tag        	\"{4}\"; " +               // {4} defines a tag associated with the concept
        "            ontsense:associateURI \"{5}\"; " +                // {5} defines the URI address
        "            ontsense:name        	\"{6}\". " +               // {6} defines a name associated with the concept
        "}}";


        ///
        /// Group of scripts that receives only a position of the event source
        ///

        /// script for a sparql insert operation with Hear sense  information
        public static readonly string INSERT_HEAR_POS =
        "PREFIX ontsense: <http://example.org/sense#>" +
        "PREFIX xsd: <http://www.w3.org/2001/XMLSchema#> " +
        "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
        "PREFIX owl: <http://www.w3.org/2002/07/owl#> " +
        "INSERT DATA" +
        "   {{" +
        "       ontsense:H{0:D10} rdf:type ontsense:RobotHear;" +        // {0} defines the hear unique identifier  Ex:  H0000002119
        "       rdf:type owl:NamedIndividual;" +
        "       ontsense:occursAt 	\"{1}\"^^xsd:dateTime;" +            // {1} defines the instant of event ocurrence. It is adherent to XSD_DATETIME = @"yyyy-MM-ddThh:mm:ss.fff";
        "       ontsense:isPositionedAt ontsense:L{0:D10} ;" +           // {0} also defines the CartesianPosition  Ex:  L0000002119
        "       ontsense:volume \"{2:G}\"^^xsd:double ;" +               // {2} defines the volume level
        "       ontsense:hasSoundType ontsense:{3} ;" +                  // {3} defines the kind of sound 
        "       ontsense:detail \"{4}\"^^xsd:string ." +                 // {4} defines a string dependent on the identity of the sound. It could be empty if no detail is associated.
        "}}";



        /// script for a sparql insert operation with smell sense  information
        public static readonly string INSERT_SMELL_POS =
        "PREFIX ontsense: <http://example.org/sense#>" +
        "PREFIX xsd: <http://www.w3.org/2001/XMLSchema#> " +
        "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
        "PREFIX owl: <http://www.w3.org/2002/07/owl#> " +
        "INSERT DATA" +
        "   {{" +
        "       ontsense:S{0:D10} rdf:type ontsense:RobotSmell;" +       // {0} defines the hear unique identifier  Ex:  S0000002111
        "       rdf:type owl:NamedIndividual;" +
        "       ontsense:occursAt 	\"{1}\"^^xsd:dateTime;" +            // {1} defines the instant of event ocurrence. It is adherent to XSD_DATETIME = @"yyyy-MM-ddThh:mm:ss.fff";
        "       ontsense:isPositionedAt ontsense:L{0:D10} ;" +           // {0} also defines the CartesianPosition  Ex:  L0000002111
        "       ontsense:hasSmellType ontsense:{2} ." +                  // {2} defines the kind of smell
        "}}";


        /// script for a sparql insert operation with touch sense  information
        public static readonly string INSERT_TOUCH_POS =
        "PREFIX ontsense: <http://example.org/sense#>" +
        "PREFIX xsd: <http://www.w3.org/2001/XMLSchema#> " +
        "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
        "PREFIX owl: <http://www.w3.org/2002/07/owl#> " +
        "INSERT DATA" +
        "   {{" +
        "       ontsense:T{0:D10} rdf:type ontsense:RobotTouch;" +          // {0} defines the hear unique identifier  Ex:  T0000002122
        "       rdf:type owl:NamedIndividual;" +
        "       ontsense:occursAt 	\"{1}\"^^xsd:dateTime;" +              // {1} defines the instant of event ocurrence. It is adherent to XSD_DATETIME = @"yyyy-MM-ddThh:mm:ss.fff";
        "       ontsense:isPositionedAt ontsense:L{0:D10} ;" +             // {0} also defines the CartesianPosition  Ex:  P0000002122
        "       ontsense:temperature    \"{2:G}\"^^xsd:double ;" +         // {2} defines the temperature 	
        "       ontsense:hardness       \"{3:G}\"^^xsd:double ;" +         // {3} defines the hardness	
        "       ontsense:moisture       \"{4:G}\"^^xsd:double ;" +         // {4} defines the moisture	
        "       ontsense:roughness      \"{5:G}\"^^xsd:double ;" +         // {5} defines the roughness	
        "       ontsense:pressure       \"{6:G}\"^^xsd:double ." +         // {6} defines the pressure
        "}}";


        ///
        /// Group of scripts that receives the exacty object responsable for the event
        ///

        /// script for a sparql insert operation with Hear sense  information
        public static readonly string INSERT_HEAR =
        "PREFIX ontsense: <http://example.org/sense#>" +
        "PREFIX xsd: <http://www.w3.org/2001/XMLSchema#> " +
        "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
        "PREFIX owl: <http://www.w3.org/2002/07/owl#> " +
        "INSERT DATA" +
        "   {{" +
        "       ontsense:H{0:D10} rdf:type ontsense:RobotHear;" +      // {0} defines the hear unique identifier  Ex:  H0000002129
        "       rdf:type owl:NamedIndividual;" +
        "       ontsense:occursAt 	\"{1}\"^^xsd:dateTime;" +          // {1} defines the instant of event ocurrence. It is adherent to XSD_DATETIME = @"yyyy-MM-ddThh:mm:ss.fff";
        "       ontsense:generateBy \"{2}\"^^xsd:long ;" +             // {2} defines the unique objectId  generated by Unity tool Ex:  1234567892
        "       ontsense:volume \"{3:G}\"^^xsd:double ;" +             // {3} defines the volume level
        "       ontsense:hasSoundType ontsense:{4} ;" +                // {4} defines the kind of sound 
        "       ontsense:detail \"{5}\"^^xsd:string ." +               // {5} defines a string dependent on the identity of the sound. It could be empty if no detail is associated.
        "}}";



        /// script for a sparql insert operation with smell sense  information
        public static readonly string INSERT_SMELL =
        "PREFIX ontsense: <http://example.org/sense#>" +
        "PREFIX xsd: <http://www.w3.org/2001/XMLSchema#> " +
        "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
        "PREFIX owl: <http://www.w3.org/2002/07/owl#> " +
        "INSERT DATA" +
        "   {{" +
        "       ontsense:S{0:D10} rdf:type ontsense:RobotSmell;" +       // {0} defines the Smell unique identifier  Ex:  S0000002222
        "       rdf:type owl:NamedIndividual;" +
        "       ontsense:occursAt 	\"{1}\"^^xsd:dateTime;" +            // {1} defines the instant of event ocurrence. It is adherent to XSD_DATETIME = @"yyyy-MM-ddThh:mm:ss.fff";
        "       ontsense:generateBy \"{2}\"^^xsd:long ;" +               // {2} defines the unique objectId  received from Unity Ex:  1234567892
        "       ontsense:hasSmellType ontsense:{3} ." +                  // {3} defines the kind of smell
        "}}";




        /// script for a sparql insert operation with touch sense  information
        public static readonly string INSERT_TOUCH =
        "PREFIX ontsense: <http://example.org/sense#>" +
        "PREFIX xsd: <http://www.w3.org/2001/XMLSchema#> " +
        "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
        "PREFIX owl: <http://www.w3.org/2002/07/owl#> " +
        "INSERT DATA" +
        "   {{" +
        "       ontsense:T{0:D10} rdf:type ontsense:RobotTouch;" +         // {0} defines the Touch unique identifier  Ex:  T0000003117
        "       rdf:type owl:NamedIndividual;" +
        "       ontsense:occursAt 	\"{1}\"^^xsd:dateTime;" +              // {1} defines the instant of event ocurrence. It is adherent to XSD_DATETIME = @"yyyy-MM-ddThh:mm:ss.fff";
        "       ontsense:generateBy \"{2}\"^^xsd:long ;" +                  // {2} defines the unique objectId  received from Unity Ex:  1234567892
        "       ontsense:temperature    \"{3:G}\"^^xsd:double ;" +         // {3} defines the temperature 	
        "       ontsense:hardness       \"{4:G}\"^^xsd:double ;" +         // {4} defines the hardness	
        "       ontsense:moisture       \"{5:G}\"^^xsd:double ;" +         // {5} defines the moisture	
        "       ontsense:roughness      \"{6:G}\"^^xsd:double ;" +         // {6} defines the roughness	
        "       ontsense:pressure       \"{7:G}\"^^xsd:double ." +         // {7} defines the pressure
        "}}";





        /// script for a sparql insert operation with taste sense  information
        public static readonly string INSERT_TASTE = 
        "PREFIX ontsense: <http://example.org/sense#>" +
        "PREFIX xsd: <http://www.w3.org/2001/XMLSchema#> " +
        "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
        "PREFIX owl: <http://www.w3.org/2002/07/owl#> " +
        "INSERT DATA" +
        "   {{" +
        "       ontsense:A{0:D10} rdf:type ontsense:RobotTaste;" +       // {0} defines the Taste unique identifier  Ex:  A0000002117
        "       rdf:type owl:NamedIndividual;" +
        "       ontsense:occursAt 	\"{1}\"^^xsd:dateTime;" +            // {1} defines the instant of event ocurrence. It is adherent to XSD_DATETIME = @"yyyy-MM-ddThh:mm:ss.fff";
        "       ontsense:generateBy \"{2}\"^^xsd:long ;" +               // {2} defines the unique objectId  received from Unity Ex:  1234567892
        "       ontsense:sweetness    \"{3:G}\"^^xsd:double ;" +         // {3} defines the temperature 	
        "       ontsense:umani        \"{4:G}\"^^xsd:double ;" +         // {4} defines the hardness	
        "       ontsense:saltiness    \"{5:G}\"^^xsd:double ;" +         // {5} defines the moisture	
        "       ontsense:bitterness   \"{6:G}\"^^xsd:double ;" +         // {6} defines the roughness	
        "       ontsense:sourness     \"{7:G}\"^^xsd:double ." +         // {7} defines the pressure
        "}}";






        /// script for a sparql insert operation with vision sense  information
        public static readonly string INSERT_VISION =
        "PREFIX ontsense: <http://example.org/sense#>" +
        "PREFIX xsd: <http://www.w3.org/2001/XMLSchema#> " +
        "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
        "PREFIX owl: <http://www.w3.org/2002/07/owl#> " +
        "INSERT DATA" +
        "   {{" +
        "       ontsense:V{0:D10} rdf:type ontsense:RobotVision;" +      // {0} defines the vision unique identifier  Ex:  V0000002900
        "       rdf:type owl:NamedIndividual;" +
        "       ontsense:occursAt 	\"{1}\"^^xsd:dateTime;" +            // {1} defines the instant of event ocurrence. It is adherent to XSD_DATETIME = @"yyyy-MM-ddThh:mm:ss.fff";
        "       ontsense:generateBy \"{2}\"^^xsd:long ." +               // {2} defines the unique objectId  received from Unity Ex:  1234567892
        "}}";








        /// script for a sparql query operation with all sense information after a given time
        /// future develoment
        /// 
        /// 
        public static readonly string QUERY_ALL_AFTER_TIME = " ";

		/// script for a sparql query operation with hear sense  information after a given time
		public static readonly string QUERY_HEAR_AFTER_TIME = " ";

		/// script for a sparql query operation with smell sense  information after a given time
		public static readonly string QUERY_SMELL_AFTER_TIME = " ";

		/// script for a sparql query operation with taste sense  information after a given time
		public static readonly string QUERY_TASTE_AFTER_TIME = " ";

		/// script for a sparql query operation with touch sense  information after a given time
		public static readonly string QUERY_TOUCH_AFTER_TIME = " ";

		/// script for a sparql query operation with vision sense  information after a given time
		public static readonly string QUERY_VISION_AFTER_TIME = " ";

		/// script for a sparql delete operation  after a given time
		public static readonly string DELETE_BEFORE_TIME = " ";


	}

}

