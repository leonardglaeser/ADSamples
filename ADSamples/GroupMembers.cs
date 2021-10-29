using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement; // MIT LICENSE Nuget
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;




namespace ADSamples
{
    public class GroupMembers
    {

        [SupportedOSPlatform("windows")]
        public static async Task Main(string[] args)
        {


            //DemoAufrufe


            Console.WriteLine("Bitte Gruppename eingeben:");
            string groupname = Console.ReadLine();

            // groupname = "beispielgruppe"

            try
            {
                List<Principal> Users = await GetUsersAD(groupname);
                Users?.ForEach(elem => Console.WriteLine(elem.SamAccountName));
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error!\n" + ex.ToString());
            }





            Console.WriteLine("Bitte distinguishedName eingeben:");
            string groupname2 = Console.ReadLine();

            // groupname2 = "CN=beispielgruppe,OU=beispielou,DC=BeispielDC"

            try
            {
                var Users2 = await GetUsersAD(groupname2,groupNameDistinguished:true);
                Users2?.ForEach(elem => Console.WriteLine(elem.SamAccountName));
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error!\n" + ex.ToString());
            }

        }


        [SupportedOSPlatform("windows")]
        static Task<List<Principal>> GetUsersAD(string groupName, bool groupNameDistinguished = false, string DomainName = null)
        {
            return Task.Run(() =>
            {
                try
                {
                    using PrincipalContext context = DomainName is null ? new PrincipalContext(ContextType.Domain) : new PrincipalContext(ContextType.Domain, DomainName);
                    using GroupPrincipal group = groupNameDistinguished ? GroupPrincipal.FindByIdentity(context, IdentityType.DistinguishedName, groupName) : GroupPrincipal.FindByIdentity(context, IdentityType.Name, groupName);
                    return group.GetMembers(true).ToList();
                }
                catch(Exception ex)
                {
                    throw new Exception("Lookup Failed",ex);
                }

            });
        }

    }

}
