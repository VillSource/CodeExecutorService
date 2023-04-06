ssh-keygen -q -f %cd%\id_rsa -b 2048 -t rsa -q -N ""

del authorized_keys
rename id_rsa.pub authorized_keys