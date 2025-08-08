# Create Toolpad App

This is a [Next.js](https://nextjs.org/) project bootstrapped with [`create-toolpad-app`](https://github.com/vercel/next.js/tree/canary/packages/create-next-app).

## Setup

Add the `NEXT_PUBLIC_API_URL` to the .env.local file:
NEXT_PUBLIC_API_URL=http://localhost:7203

## Getting Started

1. Run command to install node_modules: `yarn install`
2. Run command to start dev server: `yarn dev`

Open [http://localhost:3000] with your browser to see the result.
You BE should be also running on port [http://localhost:7203/swagger]

## Deploy on Vercel

1. Run command: `yarn build`
2. Run command if you want to test local build: `yarn start`
3. Copy artefacts `.next` folder to destination server or cloud
