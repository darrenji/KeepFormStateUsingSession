using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Extension
{
    public static class CollectionEditingHtmlExtensions
    {
        //目标生成如下格式
        //<input autocomplete="off" name="FavouriteMovies.Index" type="hidden" value="6d85a95b-1dee-4175-bfae-73fad6a3763b" />
        //<label>Title</label>
        //<input class="text-box single-line" name="FavouriteMovies[6d85a95b-1dee-4175-bfae-73fad6a3763b].Title" type="text" value="Movie 1" />
        //<span class="field-validation-valid"></span>

        public static IDisposable BeginCollectionItem<TModel>(this HtmlHelper<TModel> html, string collectionName)
        {
            //构建name="FavouriteMovies.Index"
            string collectionIndexFieldName = string.Format("{0}.Index", collectionName);
            //构建Guid字符串
            string itemIndex = GetCollectionItemIndex(collectionIndexFieldName);
            //构建带上集合属性+Guid字符串的前缀
            string collectionItemName = string.Format("{0}[{1}]", collectionName, itemIndex);

            TagBuilder indexField = new TagBuilder("input");
            indexField.MergeAttributes(new Dictionary<string, string>()
            {
                {"name", string.Format("{0}.Index", collectionName)},
                {"value", itemIndex},
                {"type", "hidden"},
                {"autocomplete", "off"}
            });
            html.ViewContext.Writer.WriteLine(indexField.ToString(TagRenderMode.SelfClosing));
            return new CollectionItemNamePrefixScope(html.ViewData.TemplateInfo, collectionItemName);
        }

        private class CollectionItemNamePrefixScope : IDisposable
        {
            private readonly TemplateInfo _templateInfo;
            private readonly string _previousPrfix;

            //通过构造函数，先把TemplateInfo以及TemplateInfo.HtmlFieldPrefix赋值给私有字段变量，并把集合属性名称赋值给TemplateInfo.HtmlFieldPrefix
            public CollectionItemNamePrefixScope(TemplateInfo templateInfo, string collectionItemName)
            {
                this._templateInfo = templateInfo;
                this._previousPrfix = templateInfo.HtmlFieldPrefix;
                templateInfo.HtmlFieldPrefix = collectionItemName;
            }

            public void Dispose()
            {
                _templateInfo.HtmlFieldPrefix = _previousPrfix;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectionIndexFieldName">比如，FavouriteMovies.Index</param>
        /// <returns>Guid字符串</returns>
        private static string GetCollectionItemIndex(string collectionIndexFieldName)
        {
            Queue<string> previousIndices = (Queue<string>)HttpContext.Current.Items[collectionIndexFieldName];
            if (previousIndices == null)
            {
                HttpContext.Current.Items[collectionIndexFieldName] = previousIndices = new Queue<string>();
                string previousIndicesValues = HttpContext.Current.Request[collectionIndexFieldName];
                if (!string.IsNullOrWhiteSpace(previousIndicesValues))
                {
                    foreach (string index in previousIndicesValues.Split(','))
                    {
                        previousIndices.Enqueue(index);
                    }
                }
            }
            return previousIndices.Count > 0 ? previousIndices.Dequeue() : Guid.NewGuid().ToString();
        }
    }
}