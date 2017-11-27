using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

public static class DocumentCoreGenerator
{
    public static void RunElements(OpenXmlElement element, IDictionary<string, string> keyValues)
    {
        var sdtElements = element.Descendants<SdtElement>().ToList();

        foreach (var sdtElement in sdtElements)
        {
            var tag = sdtElement.SdtProperties.GetFirstChild<Tag>();
            var name = tag.Val.Value;

            if (keyValues.ContainsKey(name))
            {
                var value = keyValues[name];
                if (value == null)
                    continue;

                var std = Parents<SdtElement>(tag);

                if (value.Length == 0 || value[0] != '~')
                {
                    var text = std.Descendants<Text>().First();
                    text.Text = value.ToString();
                    tag.Parent.RemoveAllChildren<ShowingPlaceholder>();
                }
            }
            else throw new Exception("keyname " + name + " not implemented");
        }
    }

    public static T Parents<T>(OpenXmlElement element)
        where T : OpenXmlElement
    {
        var runElem = element.Parent;
        do
        {
            runElem = runElem.Parent;
            if (runElem is T)
                break;
        }
        while (true);
        return (T)runElem;
    }
}