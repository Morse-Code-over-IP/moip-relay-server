%-------Input Section----------------------------------
N=2048;  %Number of sample points (N)
%N is usually a power of 2
Fm=0.1; %Maximum Doppler Frequency Shift
Fs=4; %Sampling Frequency
%Baseband Gaussian Noise Generators
mean = 0; %Mean of  Gaussian random variables
variance = 0.1; %Variance of Gaussian random variables
%------------------------------------------------------
sdev = sqrt(variance); %Standard Deviation of Gaussian RV

%In-phase Noise components
G1 = mean + sdev.*randn(1,N) ; %N i.i.d Gaussian random samples

%Quadrature-phase Noise components
G2 = mean + sdev.*randn(1,N) ; %N i.i.d Gaussian random samples

C = G1-1i*G2;

%Define Spectral characteristics of the Doppler effect in frequency domain
%Fk = Doppler Filter output
Fk = dopplerFilter(Fm,Fs,N);

%Multiply C by filter sequency Fk
U = C.*Fk;
NFFT = 2^nextpow2(length(U));
u=abs(ifft(U,NFFT)); %Take IDFT 
normalizedFading = u/max(u); %Baseband Rayleigh envelope

plot(10*log10(normalizedFading)) %plot command
title(['Rayleigh Fading with doppler effect for Fm=',num2str(Fm),'Hz']);
xlabel('Samples');
ylabel('Rayleigh Fading envelope(dB)');
axis([0 2048 -20 2]); %showing only few samples for clarity

% Write to file for input to MorseNews
dlmwrite('fading.txt',normalizedFading)
