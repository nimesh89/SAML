using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAML.Idp
{
    public class SessionManager
    {
        private readonly ConcurrentDictionary<string, List<Session>> concurrentDictionary;
        private static SessionManager instance;

        private SessionManager()
        {
            this.concurrentDictionary = new ConcurrentDictionary<string, List<Session>>();
        }

        public static SessionManager Instance {
            get
            {
                if (instance == null)
                {
                    instance = new SessionManager();
                }

                return instance;
            }
        }

        public void AddSession(string user, Session session)
        {
            if (!concurrentDictionary.ContainsKey(user))
            {
                concurrentDictionary[user] = new List<Session>();
            }

            concurrentDictionary[user].Add(session);
        }

        public List<Session> GetSessions(string user)
        {
            if (!concurrentDictionary.ContainsKey(user))
            {
                concurrentDictionary[user] = new List<Session>();
            }

            return concurrentDictionary[user];
        }

        public Session GetSessionById(string user, Guid id)
        {
            if (!concurrentDictionary.ContainsKey(user))
            {
                concurrentDictionary[user] = new List<Session>();
            }

            return concurrentDictionary[user].First(session => session.Id == id);
        }

        public List<Session> RemoveSessions(string user, Guid id)
        {
            if (!concurrentDictionary.ContainsKey(user))
            {
                return null;
            }

            List<Session> temp;
            concurrentDictionary.TryRemove(user, out temp);
            return temp;
        }
    }

    public class Session
    {
        public Guid Id { get; set; }

        public string UserAgent { get; set; }

        public string Ip { get; set; }

        public string LogoutUrl { get; set; }

        public string Issuer { get; set; }
    }
}