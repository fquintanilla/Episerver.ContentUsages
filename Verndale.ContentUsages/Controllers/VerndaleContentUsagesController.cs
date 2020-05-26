using System;
using System.Linq;
using System.Web.Mvc;
using Verndale.ContentUsages.Models;
using EPiServer;
using EPiServer.Core;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using Verndale.ContentUsages.Helpers;
using Shell = EPiServer.Shell;

namespace Verndale.ContentUsages.Controllers
{
    public class VerndaleContentUsagesController : Controller
    {
        private static readonly ILogger Logger = LogManager.GetLogger();

        public ActionResult Index()
        {
            var model = new ContentTypeUsageViewModels();

            try
            {
                var epiId = System.Web.HttpContext.Current.Request.QueryString["id"];

                var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
                var contentVersionRepository = ServiceLocator.Current.GetInstance<IContentVersionRepository>();

                var contentTypes = ContentTypeUsageHelper.ListAllContentTypes().ToList();

                var content = contentLoader.Get<IContent>(new ContentReference(epiId));
                model.ContentTypeId = content.ContentTypeID;
                model.ContentType = contentTypes?.FirstOrDefault(t => t.ID == content.ContentTypeID)?.LocalizedFullName;

                model.AllContentTypes = ContentTypeUsageHelper
                    .ListAllReferenceOfContentInstance(content.ContentLink.ID, 1, 1000, "", out _)
                    .Select(p =>
                    {
                        var contentVersion = contentVersionRepository.List(p.ContentLink)
                            ?.OrderByDescending(x => x.Saved)
                            ?.FirstOrDefault(version => version.IsMasterLanguageBranch);

                        var a = contentVersionRepository.List(p.ContentLink)
                            ?.OrderByDescending(x => x.Saved);

                        return new ContentItem
                        {
                            ContentId = p.ContentLink.ID,
                            ContentName = p.Name,
                            ContentUrl = ContentTypeUsageHelper.ResolveEditUrl(p),
                            ContentTypeId = p.ContentTypeID,
                            ContentType = contentTypes?.FirstOrDefault(t => t.ID == p.ContentTypeID)?.LocalizedFullName,
                            PublicationDate = contentVersion?.Saved,
                            ChangedBy = contentVersion?.SavedBy,
                            Language = contentVersion?.LanguageBranch
                        };
                    });


                return View(GetViewLocation("Index"), model);
            }
            catch (Exception ex)
            {
                Logger.Error("Cannot retrieve list of content types", ex);
                return View(GetViewLocation("Index"), model);
            }
        }

        public ActionResult ContentTypeList(int id)
        {
            var model = new ContentTypeUsageViewModels();

            try
            {
                var contentVersionRepository = ServiceLocator.Current.GetInstance<IContentVersionRepository>();

                var contentTypes = ContentTypeUsageHelper.ListAllContentTypes();

                model.AllContentTypes = ContentTypeUsageHelper
                    .ListAllContentOfType(id, 1, 1000, "", out _)
                    .Select(p =>
                    {
                        var contentVersion = contentVersionRepository.List(p.ContentLink)
                            ?.OrderByDescending(x => x.Saved)
                            ?.FirstOrDefault(version => version.IsMasterLanguageBranch);

                        var a = contentVersionRepository.List(p.ContentLink)
                            ?.OrderByDescending(x => x.Saved);

                        return new ContentItem
                        {
                            ContentId = p.ContentLink.ID,
                            ContentName = p.Name,
                            ContentUrl = ContentTypeUsageHelper.ResolveEditUrl(p),
                            ContentType = contentTypes?.FirstOrDefault(t => t.ID == p.ContentTypeID)?.LocalizedFullName,
                            PublicationDate = contentVersion?.Saved,
                            ChangedBy = contentVersion?.SavedBy,
                            Language = contentVersion?.LanguageBranch
                        };
                    });


                return View(GetViewLocation("ContentTypeList"), model);
            }
            catch (Exception ex)
            {
                Logger.Error("Cannot retrieve list of content types", ex);
                return View(GetViewLocation("ContentTypeList"), model);
            }
        }

        private static string GetViewLocation(string viewName)
        {
            return $"{Shell.Paths.ProtectedRootPath}Verndale.ContentUsages/Views/{viewName}.cshtml";
        }
    }
}