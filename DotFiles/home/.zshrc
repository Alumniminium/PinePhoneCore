export TERM=xterm-256color
export ZSH="${HOME}/.oh-my-zsh"
ZSH_THEME="lambda-mod"
source $HOME/.zprofile
source $ZSH/oh-my-zsh.sh

alias yt='f() { mpv $1    ytdl-format="bestvideo[height<=?1080][vcodec!=vp9]+bestaudio/best" };f'
alias chmox='chmod'
alias cls='clear && ls'
alias ncdu='ncdu --exclude="/mnt" --exclude="/storage"'
alias upack='dtrx'
alias ls='exa --icons -l --ignore-glob="*~" --group-directories-first'
alias dfc='dfc -WwT -p /dev,alumni,root'
alias yay='yay --nopgpfetch --mflags --skippgpcheck'
alias trash='rmtrash'
alias rm='echo "use trash"; rm -i'
alias curl='curl --no-progress-meter'

alias ytdla="youtube-dl -f 'bestaudio[ext=m4a]'"
alias ytdlv="youtube-dl -f 'bestvideo[height<=?1080][vcodec!=vp9]+bestaudio/best'"

#THE WELCOME MESSAGE
zshstartup.sh
