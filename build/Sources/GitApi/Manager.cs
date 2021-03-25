using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GitApi
{
    public class Manager
    {
        public static int limitRemain { get; private set; } = 0;
        public static int limitTotal { get; private set; } = 0;
        public static DateTime limitUpdateTime { get; private set; }

        private static string secretToken;

        public static void setToken(string token)
        {
            secretToken = token;
        }

        public static void resetToken()
        {
            secretToken = "";
        }

        public static string toApiUrl(string gitUrl)
        {
            if (isApiUrl(gitUrl))
                return gitUrl;
            var tokens = gitUrl.Split('/');
            if (tokens.Length == 0)
                throw new ArgumentException("Invalid url");
            var username = "";
            var repo = "";
            int i = 1;
            if (tokens[0] == "http:" || tokens[0] == "https:")
                i = 3;
            for (;i<tokens.Length;i++)
            {
                if (string.IsNullOrEmpty(username))
                {
                    username = tokens[i];
                    continue;
                }
                if (string.IsNullOrEmpty(repo))
                {
                    repo = tokens[i];
                    continue;
                }
                break;
            }
            
            if(string.IsNullOrEmpty(username))
            {
                return "https://api.github.com/";
            }

            if (string.IsNullOrEmpty(repo))
            {
                return string.Format("https://api.github.com/users/{0}",username);
            }

            return string.Format("https://api.github.com/repos/{0}/{1}", username,repo);

        }

        private static bool isApiUrl(string url)
        {
            var tokens = url.Split('/');
            if (tokens.Length == 0)
                return false;
            if(tokens[0]=="http:" || tokens[0]=="https:")
            {
                if (tokens.Length < 4)//"https:","empty token","api.github.com"
                    return false;
                if (tokens[2] == "api.github.com")
                    return true;
            }
            if(tokens[0]=="api.github.com")
                return true;
            return false;
        }
        private static  WebResponse makeRequestObject(string requestStr)
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(requestStr);
            myHttpWebRequest.UserAgent = "request";
            if(!string.IsNullOrEmpty(secretToken))
            {
                myHttpWebRequest.Headers.Add("Authorization", string.Format("token {0}", secretToken));
            }
            return myHttpWebRequest.GetResponse();
        }
        public static T doRequest<T>(string request) where T : class, new()
        {
            var res = makeRequestObject(request);
            var hdr = res.Headers;
            limitRemain = Int32.Parse(hdr["X-Ratelimit-Remaining"]);
            limitTotal = Int32.Parse(hdr["X-Ratelimit-Limit"]);
            limitUpdateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
            limitUpdateTime = limitUpdateTime.AddSeconds(Int64.Parse(hdr["X-Ratelimit-Reset"])).ToLocalTime();

            using (var reader = new StreamReader( res.GetResponseStream()))
            {
                
                var deserializedObj = new T();
                var ser = new DataContractJsonSerializer(deserializedObj.GetType());
                deserializedObj = ser.ReadObject(reader.BaseStream) as T;
                return deserializedObj;
            }
        }

        public static User getUserInfo(string login)
        {
            return doRequest<User>(string.Format("https://api.github.com/users/{0}", login));
        }

        public static List<Branch> getBranchesList(Repo repo)
        {
            return doRequest<List<Branch>>(string.Format("https://api.github.com/repos/{0}/{1}/branches", repo.owner.login,repo.name));
        }
        public static Branch getBranchInfo(Repo repo, Branch branch)
        {
            return doRequest<Branch>(string.Format("https://api.github.com/repos/{0}/{1}/branches/{2}", repo.owner.login, repo.name,branch.name));
        }

        public static List<Repo> getUserRepos(User user)
        {
            return doRequest<List<Repo>>(string.Format("https://api.github.com/users/{0}/repos", user?.login));
        }
        
        public static List<Commit> getCommits(Repo repository)
        {
            return doRequest<List<Commit>>(string.Format("https://api.github.com/repos/{0}/{1}/commits", repository?.owner.login, repository?.name));
        }

        public static Repo getRepoInfo(Repo repo)
        {
            return doRequest<Repo>(string.Format("https://api.github.com/repos/{0}/{1}", repo?.owner.login, repo?.name));
        }

        public static Tree getTree(Commit commit, bool recursive)
        {
            return doRequest<Tree>(commit.tree.url+string.Format("?recursive={0}",recursive));
        }

        public static Blob getBlob(TreeNode node)
        {
            return doRequest<Blob>(node.url);
        }

    }
}
