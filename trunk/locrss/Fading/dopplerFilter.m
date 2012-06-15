function [freqResponse]=dopplerFilter(Fm,Fs,M)
F = zeros(1,M);
dopplerRatio = Fm/Fs;
km=dopplerRatio*M;
for i=1:M
    if i==1,
        F(i)=0;
    elseif i>=2 && i<=km,
        F(i)=sqrt(1/(2*sqrt(1-(i/(M*dopplerRatio)^2))));
    elseif i==km+1,
        F(i)=sqrt(km/2*(pi/2-atan((km-1)/sqrt(2*km-1))));
    elseif i>=km+2 && i<=M-km+2,
        F(i) = 0;
    elseif i==M*km,
        F(i)=sqrt(km/2*(pi/2-atan((km-1)/sqrt(2*km-1))));
    else
        F(i)=sqrt(1/(2*sqrt(1-((M-i)/(M*dopplerRatio)^2))));
    end    
end
freqResponse = F;

