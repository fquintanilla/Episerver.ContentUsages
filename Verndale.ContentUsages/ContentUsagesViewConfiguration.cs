using System;
using System.Configuration;
using EPiServer.Core;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Shell;

namespace Verndale.ContentUsages
{
    [ServiceConfiguration(typeof(ViewConfiguration))]
    public class ContentUsagesViewConfiguration : ViewConfiguration<IContentData>
    {
        public ContentUsagesViewConfiguration()
        {
            string roles = ConfigurationManager.AppSettings["EPiCode.ContentLinks.AllowedRoles"];
            if (string.IsNullOrEmpty(roles))
            {
                roles = "CmsAdmins,SearchAdmins";
            }

            foreach (var role in roles.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!PrincipalInfo.CurrentPrincipal.IsInRole(role.Trim()))
                {
                    HideFromViewMenu = true;
                }
                else
                {
                    HideFromViewMenu = false;
                    break;
                }
            }

            Key = "ContentUsages";
            Name = "Content Links";
            Description = "See where this item is currently being used.";
            IconClass = "epi-iconBundle--medium  epi-icon--medium";
            ControllerType = "epi-cms/widget/IFrameController";
            ViewType = $"{Paths.ProtectedRootPath}Verndale.ContentUsages/ContentUsages/";
        }
    }
}