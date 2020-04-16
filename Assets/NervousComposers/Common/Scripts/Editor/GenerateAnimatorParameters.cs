using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Jusw85.Common
{
    public class GenerateAnimatorParameters
    {
        internal static string searchPath = "Assets/Project";
        internal static string outFolder = "scripts/auto-generated/";
        internal static string fileName = "AnimatorParams";

        private static string NAMESPACE = "k";
        private static string DIGIT_PREFIX = "k";

        [MenuItem("Tools/Nervous Composers/Generate Animator Parameters", false, 110)]
        private static void GenerateAnimatorParametersMenu()
        {
            DoGenerateAnimatorParameters();
        }

        private static void DoGenerateAnimatorParameters()
        {
            string[] guids = AssetDatabase.FindAssets("t:AnimatorController", new[] {searchPath});
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                AnimatorController controller = (AnimatorController) AssetDatabase.LoadMainAssetAtPath(path);
                string content = GetClassContent(controller.parameters);

                string folderPath = Application.dataPath + "/" + outFolder + "/";
                string fullFileName = fileName + ".cs";
                File.WriteAllText(folderPath + fullFileName, content);
                AssetDatabase.ImportAsset("Assets/" + outFolder + fullFileName, ImportAssetOptions.ForceUpdate);
            }
        }

        private static string GetClassContent(AnimatorControllerParameter[] parms)
        {
            var output = "";
            output += "//This class is auto-generated do not modify\n";
            output += "namespace " + NAMESPACE + "\n";
            output += "{\n";
            output += "\tpublic static class " + fileName + "\n";
            output += "\t{\n";

            foreach (AnimatorControllerParameter parm in parms)
            {
                output += "\t\t" + buildConstVariable(parm.name, "", parm.nameHash.ToString()) + "\n";
            }

            output += "\t}\n";
            output += "}";

            return output;
        }

        private static string buildConstVariable(string varName, string suffix = "", string value = null)
        {
            value = value ?? varName;
            return "public const int " + toUpperCaseWithUnderscores(varName) + suffix + " = " + value +
                   ";";
        }

        private static string toUpperCaseWithUnderscores(string input)
        {
            input = input.Replace("-", "_");
            input = Regex.Replace(input, @"\s+", "_");

            // make camel-case have an underscore between letters
            Func<char, int, string> func = (x, i) =>
            {
                if (i > 0 && char.IsUpper(x) && char.IsLower(input[i - 1]))
                    return "_" + x.ToString();
                return x.ToString();
            };
            input = string.Concat(input.Select(func).ToArray());

            // digits are a no-no so stick prefix in front
            if (char.IsDigit(input[0]))
                return DIGIT_PREFIX + input.ToUpper();
            return input.ToUpper();
        }

        internal static void DefaultSettings()
        {
            searchPath = "Assets/Project";
            outFolder = "scripts/auto-generated/";
            fileName = "AnimatorParams";
        }
    }
}