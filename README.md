# foxtaur-2d
An attempt to implement ARDF sportsment tracker (in 2D)

# Dependencies

Create directories:

/opt/foxtaur/logs - for server logs

# Build and run on Docker

# Infrastructure

Duild image: $ docker build -f dockerfile-foxtaur-infrastructure -t foxtaur-infrastructure-postgres .

Run container: $ docker-compose up -d

# Main image

Build image: $ docker build -f dockerfile-foxtaur-main -t foxtaur-main .

Run container: Go to Docker/foxtaur-main and invoke: $ docker-compose up -d

# Configure reverse proxy
Configure reverse proxy in a such way (example for Apache):

##############################################################################
#                               api.foxtaur.me                               #
##############################################################################
<VirtualHost *:80>
    ServerName api.foxtaur.me
    ServerAdmin whitefossa@protonmail.com
    ErrorLog "/webroot/vhosts/api.foxtaur.me/logs/error.log"
    CustomLog "/webroot/vhosts/api.foxtaur.me/logs/access.log" combined

    ProxyPass / http://127.0.0.1:5035/
    ProxyPassReverse / http://127.0.0.1:5035/
    ProxyRequests Off
</VirtualHost>

<VirtualHost *:443>
    ServerName api.foxtaur.me
    ServerAdmin whitefossa@protonmail.com
    ErrorLog "/webroot/vhosts/api.foxtaur.me/logs/error.log"
    CustomLog "/webroot/vhosts/api.foxtaur.me/logs/access.log" combined

    ProxyPass / http://127.0.0.1:5035/
    ProxyPassReverse / http://127.0.0.1:5035/
    ProxyRequests Off

	SSLEngine on
	SSLCertificateFile "/etc/letsencrypt/live/api.foxtaur.me/cert.pem"
	SSLCertificateKeyFile "/etc/letsencrypt/live/api.foxtaur.me/privkey.pem"
	SSLCertificateChainFile "/etc/letsencrypt/live/api.foxtaur.me/fullchain.pem"
</VirtualHost>
