using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using Core.UsuallyCommon;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;

namespace Core.Services
{
    // <summary>
    /// Csharp 字符串识别
    /// </summary>
    public class CsharpParser
    {

        public static List<Assembly> GetAssemblies(string path)
        {
            List<Assembly> allAssemblies = new List<Assembly>();
            path = string.IsNullOrEmpty(path) ? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) : path;
            foreach (string dll in Directory.GetFiles(path, "*.dll"))
                allAssemblies.Add(Assembly.LoadFile(dll));
            return allAssemblies;
        }

        public static List<Type> GetInterfaces<T>(string path)
        {
            var allAssemblies = GetAssemblies(path);
            var instance = allAssemblies
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(T))))
                .ToList();
            return instance;
        }

        public static List<Type> GetInterfaces(string path)
        {
            var allAssemblies = GetAssemblies(path);
            var instance = allAssemblies
                .SelectMany(a => a.GetTypes().Where(t => t.IsInterface))
                .ToList();
            return instance;
        }

        public static List<Type> GetEnums(string path)
        {
            var allAssemblies = GetAssemblies(path);
            var instance = allAssemblies
                .SelectMany(a => a.GetTypes().Where(t => t.IsEnum))
                .ToList();
            return instance;
        }

        public SemanticModel semanticModel { get; set; }

        public static PortableExecutableReference Mscorlib { get; set; }

        public SyntaxNode roots { get; set; }
        /// <summary>
        /// 根据路劲
        /// </summary>
        /// <param name="Path"></param>
        public CsharpParser(string Path)
        {
            var context = Path.GetFileContext();
            var syntaxTree = CSharpSyntaxTree.ParseText(context);
            roots = syntaxTree.GetRoot();
            if (Mscorlib == null)
                Mscorlib = PortableExecutableReference.CreateFromFile(typeof(object).Assembly.Location);
            var compilation = CSharpCompilation.Create("MyCompilation",
                syntaxTrees: new[] { syntaxTree }, references: new[] { Mscorlib });
            semanticModel = compilation.GetSemanticModel(syntaxTree);


        }


        /// <summary>
        /// 获取所有类
        /// </summary>
        /// <returns></returns>
        public List<ClassDeclarationSyntax> GetClasses()
        {
            return roots.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();
        }

        /// <summary>
        /// 获取Class
        /// </summary>
        /// <returns></returns>
        public ClassDeclarationSyntax GetClass()
        {
            return roots.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
        }

        /// <summary>
        /// 获取方法Commenet
        /// </summary>
        /// <param name="method"></param>
        public static String GetClassComment(ClassDeclarationSyntax classes)
        {
            var comments = string.Empty;
            if (classes.HasStructuredTrivia)
            {
                var comment = classes.GetLeadingTrivia().
                 ToSyntaxTriviaList().Where(x => x.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia || x.Kind() == SyntaxKind.SingleLineCommentTrivia
                 ).FirstOrDefault();
                if (comment.GetStructure() != null)
                {
                    comments = comment.GetStructure().GetText().ToString().Replace("///", string.Empty);
                    comments = Regex.Replace(comments, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                    comments = Regex.Replace(comments, @"\r\n", "", RegexOptions.IgnoreCase);
                }
            }
            return comments.Trim();
        }

        /// <summary>
        /// 获取最后文本
        /// </summary>
        /// <returns></returns>
        public string GetFullStrig()
        {
            return roots.ToFullString();
        }


        #region Method 
        /// <summary>
        /// 获取方法Commenet
        /// </summary>
        /// <param name="method"></param>
        public static String GetMethodComment(MethodDeclarationSyntax method)
        {
            var comments = string.Empty;
            if (method.HasStructuredTrivia)
            {
                var comment = method.GetLeadingTrivia().
                 ToSyntaxTriviaList().Where(x => x.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia || x.Kind() == SyntaxKind.SingleLineCommentTrivia
                 ).FirstOrDefault();
                if (comment.GetStructure() != null)
                {
                    comments = comment.GetStructure().GetText().ToString().Replace("///", string.Empty);
                    comments = Regex.Replace(comments, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                    comments = Regex.Replace(comments, @"\r\n", "", RegexOptions.IgnoreCase);
                }
            }
            return comments.Trim();
        }
        #endregion

        #region Property 
        /// <summary>
        /// 获取属性Commenet
        /// </summary>
        /// <param name="property"></param>
        public static String GetPropertyComment(PropertyDeclarationSyntax property)
        {
            var comments = string.Empty;
            if (property.HasStructuredTrivia)
            {
                var comment = property.GetLeadingTrivia().
                 ToSyntaxTriviaList().Where(x => x.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia || x.Kind() == SyntaxKind.SingleLineCommentTrivia
                 ).FirstOrDefault();
                if (comment.GetStructure() != null)
                {
                    comments = comment.GetStructure().GetText().ToString().Replace("///", string.Empty);
                    comments = Regex.Replace(comments, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                    comments = Regex.Replace(comments, @"\r\n", "", RegexOptions.IgnoreCase);
                }

            }
            return comments.Trim();
        }

        /// <summary>
        /// 获取类的属性
        /// </summary>
        /// <param name="clssses"></param>
        /// <returns></returns>
        public List<PropertyDeclarationSyntax> GetClassProperty(ClassDeclarationSyntax classes)
        {
            return classes.DescendantNodes().OfType<PropertyDeclarationSyntax>().ToList();
        }


        /// <summary>
        /// 获取类的属性
        /// </summary>
        /// <param name="clssses"></param>
        /// <returns></returns>
        public List<CsharpProperty> GetCsharpClassProperty(ClassDeclarationSyntax classes)
        {
            List<CsharpProperty> response = new List<CsharpProperty>();
            GetClassProperty(classes).ForEach(x => {
                var maxlentharr = x.AttributeLists.ToList().FirstOrDefault(y => y.Attributes.Any(u => u.Name.ToString() == "MaxLength"));
                Int64 maxlength = 0;
                if (maxlentharr != null)
                    maxlength = maxlentharr.Attributes.FirstOrDefault().ArgumentList.Arguments.ToString().ToInt64();
                response.Add(new CsharpProperty()
                {
                    PropertyName = x.Identifier.Text,
                    PropertyComment = GetPropertyComment(x),
                    PropertyType = x.Type.ToStringExtension(),
                    MaxLength = x.Type.ToStringExtension().ToLower() == "string" ? maxlength : 0,
                    IsRequire = !(x.Type.ToStringExtension().IndexOf("?") > -1),
                    Table = classes.Identifier.Text
                });
            });

            return response;
        }

        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="classes"></param>
        /// <param name="property"></param>
        public void AddProperty(ClassDeclarationSyntax classes, CsharpProperty property)
        {
            var updatedClass = classes.AddMembers(CreateProperty(property));
            roots = roots.ReplaceNode(classes, updatedClass).NormalizeWhitespace();
        }

        /// <summary>
        /// 替换属性
        /// </summary>
        /// <param name="classes"></param>
        /// <param name="property"></param>
        public void ReplaceProperty(ClassDeclarationSyntax classes, CsharpProperty property, CsharpProperty newProperty)
        {
            var rproperty = GetClassProperty(classes).FirstOrDefault(x => x.Identifier.Text == property.PropertyName);
            var updatedClass = classes.ReplaceNode(rproperty, CreateProperty(newProperty));
            roots = roots.ReplaceNode(classes, updatedClass).NormalizeWhitespace();
        }

        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="classes"></param>
        /// <param name="property"></param>
        public void RemoveProperty(ClassDeclarationSyntax classes, CsharpProperty property)
        {
            var rproperty = GetClassProperty(classes).FirstOrDefault(x => x.Identifier.Text == property.PropertyName);
            var updatedClass = classes.RemoveNode(rproperty, SyntaxRemoveOptions.KeepNoTrivia);
            roots = roots.ReplaceNode(classes, updatedClass).NormalizeWhitespace();
        }


        /// <summary>
        /// 创建新属性
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public PropertyDeclarationSyntax CreateProperty(CsharpProperty property)
        {
            var documentcomment = GeneratorComment(property.PropertyComment);
            var propertyDeclaration = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(property.PropertyType), property.PropertyName)
              // .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
              .AddAccessorListAccessors(
                  SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                  SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))).WithModifiers(
               SyntaxFactory.TokenList(
                   new[]{
                        SyntaxFactory.Token(
                            SyntaxFactory.TriviaList(
                                SyntaxFactory.Trivia(documentcomment)), // xmldoc
                                SyntaxKind.PublicKeyword, // original 1st token
                                SyntaxFactory.TriviaList())
                                //, SyntaxFactory.Token(SyntaxKind.PrivateKeyword)
                   }));
            return propertyDeclaration;

        }



        /// <summary>
        /// 生成文档
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public DocumentationCommentTriviaSyntax GeneratorComment(string comment)
        {
            return SyntaxFactory.DocumentationCommentTrivia(
        SyntaxKind.SingleLineDocumentationCommentTrivia,
        SyntaxFactory.List<XmlNodeSyntax>(
            new XmlNodeSyntax[]{
            SyntaxFactory.XmlText()
            .WithTextTokens(
                SyntaxFactory.TokenList(
                    SyntaxFactory.XmlTextLiteral(
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.DocumentationCommentExterior("///")),
                        " ",
                        " ",
                        SyntaxFactory.TriviaList()))),
            SyntaxFactory.XmlElement(
                SyntaxFactory.XmlElementStartTag(
                    SyntaxFactory.XmlName(
                        SyntaxFactory.Identifier("summary"))),
                SyntaxFactory.XmlElementEndTag(
                    SyntaxFactory.XmlName(
                        SyntaxFactory.Identifier("summary"))))
             .WithContent(
                                        SyntaxFactory.SingletonList<XmlNodeSyntax>(
                                            SyntaxFactory.XmlText()
                                            .WithTextTokens(
                                                SyntaxFactory.TokenList(
                                                    new []{
                                                        SyntaxFactory.XmlTextNewLine(
                                                            SyntaxFactory.TriviaList(),
                                                            @"
",
                                                            @"
",
                                                            SyntaxFactory.TriviaList()),
                                                        SyntaxFactory.XmlTextLiteral(
                                                            SyntaxFactory.TriviaList(
                                                                SyntaxFactory.DocumentationCommentExterior(
                                                                    @"///")),
                                                            $"{comment}",
                                                            $"{comment}",
                                                            SyntaxFactory.TriviaList()),
                                                        SyntaxFactory.XmlTextNewLine(
                                                            SyntaxFactory.TriviaList(),
                                                            @"
",
                                                            @"
",
                                                            SyntaxFactory.TriviaList()),
                                                        SyntaxFactory.XmlTextLiteral(
                                                            SyntaxFactory.TriviaList(
                                                                SyntaxFactory.DocumentationCommentExterior(
                                                                    @"///")),
                                                            @" ",
                                                            @" ",
                                                            SyntaxFactory.TriviaList())})))),
                                    SyntaxFactory.XmlText()
                                    .WithTextTokens(
                                        SyntaxFactory.TokenList(
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                @"
",
                                                @"
",
                                                SyntaxFactory.TriviaList())))}));
        }
        #endregion
    }
}
