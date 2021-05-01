# PinePhone
PinePhone scripts and apps I wrote/am writing for SXMO

## ppwake
will turn on wifi, check if my network is available, if it is, connect, sync music from homeserver, turn off wifi
                                                     if it isnt, connect to 4G, update dynamic dns, turn off 4G
only runs between 11pm and 4am, exits without doing anything otherwise

usage:
execute 
`dotnet ppwake.dll`
in
`~/.config/sxmo/hooks/postwake`

## pplauncher
experimental avalonia application launcher, not working. Avalonia depends on netcore <5 and that won't run on aarch64-musl