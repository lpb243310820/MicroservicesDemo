﻿using System;
using System.Text;
using RabbitMQ.Client;

namespace CapRabbitMq
{
    public class AliyunMechanism : AuthMechanism
    {
        public byte[] handleChallenge(byte[] challenge, IConnectionFactory factory)
        {
            if (factory is ConnectionFactory)
            {
                ConnectionFactory cf = factory as ConnectionFactory;
                return Encoding.UTF8.GetBytes("\0" + getUserName(cf) + "\0" + AliyunUtils.getPassword(cf.Password));
            }
            else
            {
                throw new InvalidCastException("need ConnectionFactory");
            }
        }

        private string getUserName(ConnectionFactory cf)
        {
            string ownerResourceId;
            try
            {
                string[] sArray = cf.HostName.Split('.');
                ownerResourceId = sArray[0];
            }
            catch (Exception)
            {
                throw new InvalidProgramException("hostName invalid");
            }
            Console.WriteLine(ownerResourceId);
            return AliyunUtils.getUserName(cf.UserName, ownerResourceId);
        }
    }
}