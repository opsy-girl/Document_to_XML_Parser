# Document_to_XML_Parser
Code to convert PDF files to XML based on formal methods

 # Step 1: (Slicing) 
 A function defined by ITextSharp software library is used for slicing
the input document into “lines of text”. The function takes as a parameter the
input document and a set of document predefined styles e.g. font bold, font size,
top space, bottom space, left align, and right align. 

# Step 2: (Tokenization) 
“lines of text” are matched with the corresponding
REGEX of text string symbol. The “lines of text” are identified as any text
string symbols of title(tt), author(ta), supervisor name(ts), chapter paragraphs ((tp)
and so on. 

# Step 3: (Parsing) 
Valid text string symbolic tokens are organized into a nested array object
using production rules.

# Step 4: (Serialization) 
The function to serialize nested array object to XmlFile which
takes as input the nested array object and output as XML file using C# inbuilt serialize function. 

# Step 5: Return output (XML file)
