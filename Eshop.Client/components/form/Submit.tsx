import Button from "@mui/material/Button";

type Props = {
  text: string;
};
const Submit = ({ text }: Props) => {
  return (
    <Button type="submit" variant="contained" color="primary">
      {text}
    </Button>
  );
};

export default Submit;
