import {
  ActionIcon,
  Anchor,
  AppShell,
  Avatar,
  Box,
  Burger,
  Code,
  Container,
  Divider,
  Group,
  Header,
  List,
  MediaQuery,
  Navbar,
  Space,
  Table,
  Text,
  ThemeIcon,
  Title,
  UnstyledButton,
  useMantineColorScheme,
  useMantineTheme,
} from "@mantine/core";
import { useState } from "react";
import {
  ChartRadar,
  ChevronLeft,
  ChevronRight,
  CirclePlus,
  Map,
  MoonStars,
  Mouse,
  QuestionMark,
  Stack2,
  Sun,
} from "tabler-icons-react";
import MapColorizer from "./mapcolorizer";

interface MainLinkProps {
  icon: React.ReactNode;
  color: string;
  label: string;
  href: string;
}

const MainLink = ({ icon, color, label, href }: MainLinkProps) => {
  const { colorScheme } = useMantineColorScheme();
  return (
    <UnstyledButton
      component="a"
      href={href}
      sx={(theme) => ({
        display: "block",
        width: "100%",
        padding: theme.spacing.xs,
        borderRadius: theme.radius.sm,
        color:
          theme.colorScheme === "dark" ? theme.colors.dark[0] : theme.black,

        "&:hover": {
          backgroundColor:
            theme.colorScheme === "dark"
              ? theme.colors.dark[6]
              : theme.colors.gray[0],
        },
      })}
    >
      <Group>
        <ThemeIcon
          color={color}
          variant={colorScheme === "dark" ? "light" : "filled"}
        >
          {icon}
        </ThemeIcon>

        <Text size="sm">{label}</Text>
      </Group>
    </UnstyledButton>
  );
};

const linkData = [
  {
    icon: <Map size={16} />,
    color: "violet",
    label: "Map Colorizer",
    href: "#map",
  },
  {
    icon: <Mouse size={16} />,
    color: "blue",
    label: "Controls",
    href: "#controls",
  },
  {
    icon: <QuestionMark size={16} />,
    color: "teal",
    label: "About",
    href: "#about",
  },
  {
    icon: <ChartRadar size={16} />,
    color: "red",
    label: "Algorithms",
    href: "#algorithms",
  },
  {
    icon: <Stack2 size={16} />,
    color: "yellow",
    label: "Technology",
    href: "#tech",
  },
  {
    icon: <CirclePlus size={16} />,
    color: "gray",
    label: "What's Next?",
    href: "#next",
  },
];

const User = () => {
  const theme = useMantineTheme();
  return (
    <Box
      sx={{
        paddingTop: theme.spacing.sm,
        borderTop: `1px solid ${
          theme.colorScheme === "dark"
            ? theme.colors.dark[4]
            : theme.colors.gray[2]
        }`,
      }}
    >
      <UnstyledButton
        component="a"
        target="_blank"
        href="https://github.com/technologeli"
        sx={{
          display: "block",
          width: "100%",
          padding: theme.spacing.xs,
          borderRadius: theme.radius.sm,
          color:
            theme.colorScheme === "dark" ? theme.colors.dark[0] : theme.black,

          "&:hover": {
            backgroundColor:
              theme.colorScheme === "dark"
                ? theme.colors.dark[6]
                : theme.colors.gray[0],
          },
        }}
      >
        <Group>
          <Avatar src="https://github.com/technologeli.png" radius="xl" />
          <Box sx={{ flex: 1 }}>
            <Text size="sm" weight={500}>
              Eli
            </Text>
            <Text color="dimmed" size="xs">
              technologeli
            </Text>
          </Box>

          {theme.dir === "ltr" ? (
            <ChevronRight size={18} />
          ) : (
            <ChevronLeft size={18} />
          )}
        </Group>
      </UnstyledButton>
    </Box>
  );
};

const Page = () => {
  const { colorScheme, toggleColorScheme } = useMantineColorScheme();
  const theme = useMantineTheme();
  const [opened, setOpened] = useState(false);
  return (
    <AppShell
      styles={{
        main: {
          background:
            theme.colorScheme === "dark"
              ? theme.colors.dark[8]
              : theme.colors.gray[0],
        },
      }}
      fixed
      padding="md"
      navbarOffsetBreakpoint="sm"
      header={
        <Header height={60}>
          <Group sx={{ height: "100%" }} px={20} position="apart">
            <Group>
              <MediaQuery largerThan="sm" styles={{ display: "none" }}>
                <Burger
                  opened={opened}
                  onClick={() => setOpened((o) => !o)}
                  size="sm"
                  color={theme.colors.gray[6]}
                  mr="xl"
                />
              </MediaQuery>
              <Title pt={10} align="center">
                Map Colorizer
              </Title>
            </Group>

            <ActionIcon
              variant="default"
              onClick={() => toggleColorScheme()}
              size={30}
            >
              {colorScheme === "dark" ? (
                <Sun size={16} />
              ) : (
                <MoonStars size={16} />
              )}
            </ActionIcon>
          </Group>
        </Header>
      }
      navbar={
        <Navbar
          width={{ sm: 250, lg: 250 }}
          p="xs"
          hiddenBreakpoint="sm"
          hidden={!opened}
        >
          <Navbar.Section grow>
            {linkData.map((link) => (
              <MainLink {...link} key={link.label} />
            ))}
          </Navbar.Section>
          <Navbar.Section>
            <User />
          </Navbar.Section>
        </Navbar>
      }
    >
      <Container size="md" px="xs">
        <div id="map"></div>
        <Space h="md" />
        <Divider my="lg" />
        <Space h="md" />

        <MapColorizer />

        <div id="controls"></div>
        <Space h="md" />
        <Divider my="lg" />
        <Space h="md" />

        <section>
          <Title order={2}>Controls</Title>

          <Table striped highlightOnHover>
            <thead>
              <tr>
                <th>
                  <Text>Action</Text>
                </th>
                <th>
                  <Text>on a Point</Text>
                </th>
                <th>
                  <Text>on the Map</Text>
                </th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>
                  <Text style={{ whiteSpace: "nowrap" }}>
                    <Code color="blue">Left Click</Code>
                  </Text>
                </td>
                <td>
                  <Text>
                    Close the current line. If there is no current line, select
                    a line that uses that point. Click again to cycle through
                    the lines.
                  </Text>
                </td>
                <td>
                  <Text>Create a new line and place its first point.</Text>
                </td>
              </tr>
              <tr>
                <td>
                  <Text style={{ whiteSpace: "nowrap" }}>
                    <Code color="red">Right Click</Code>
                  </Text>
                </td>
                <td>
                  <Text>
                    Delete that point. All connected lines will connect the
                    previous and next points.
                  </Text>
                </td>
                <td>
                  <Text>Deselect the current line.</Text>
                </td>
              </tr>
              <tr>
                <td>
                  <Text style={{ whiteSpace: "nowrap" }}>
                    <Code color="green">Drag</Code>
                  </Text>
                </td>
                <td>
                  <Text>Move the point.</Text>
                </td>
                <td>
                  <Text>No functionality.</Text>
                </td>
              </tr>
            </tbody>
          </Table>
          <Space h="md" />
          <Text>
            If you would like to create a new line that starts on an existing
            point, click the "Create Line" button and then click on the desired
            point. You will have to manually close the line by selecting it and
            clicking the "Toggle Loop" button.
          </Text>
        </section>

        <div id="about"></div>
        <Space h="md" />
        <Divider my="lg" />
        <Space h="md" />

        <section>
          <Title order={2}>About</Title>
          <Text>
            This application demonstrates the{" "}
            <Anchor
              target="_blank"
              href="https://en.wikipedia.org/wiki/Four_color_theorem"
            >
              four color theorem
            </Anchor>
            , which states that a map can be colored in at most four colors
            without having two bordering regions use the same color. This
            application does not prove the theorem, but it is a nice tool to
            examine it.
          </Text>
          <Space h="md" />
          <Text>
            If you can create a map where the four color theorem is not valid,
            you have likely discovered an error in my program and not an error
            in the theorem. Known occurrences include creating complex polygons,
            such as ones that intersect with itself.
          </Text>
        </section>

        <div id="algorithms"></div>
        <Space h="md" />
        <Divider my="lg" />
        <Space h="md" />

        <section>
          <Title order={2}>Algorithms</Title>
          <Text>
            The application uses a variety of algorithms for various
            functionality. This includes the ear clipping algorithm, a border
            checking algorithm, and a recursive backtracking color algorithm.
          </Text>

          <Table striped highlightOnHover>
            <thead>
              <tr>
                <th>
                  <Text>Algorithm Name</Text>
                </th>
                <th>
                  <Text>Description</Text>
                </th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>
                  <Text>Ear Clipping</Text>
                </td>
                <td>
                  <Text>
                    The Ear Clipping algorithm is used to cut simple polygons
                    into triangles. I learned about it from{" "}
                    <Anchor
                      target="_blank"
                      href="https://www.youtube.com/watch?v=QAdfkylpYwc"
                    >
                      this video
                    </Anchor>
                    . In this application, it is used to color the regions,
                    which are simple polygons.
                  </Text>
                </td>
              </tr>
              <tr>
                <td>
                  <Text>Border Checking</Text>
                </td>
                <td>
                  <Text>
                    The Border Checking algorithm is something I produced on my
                    own. Given a list of regions, which are composed of a list
                    of points, this algorithm determines which regions border
                    which.
                  </Text>
                </td>
              </tr>
              <tr>
                <td>
                  <Text>Recursive Backracking Color</Text>
                </td>
                <td>
                  <Text>
                    Though I produced the Recursive Backtracking Color algorithm
                    on my own, I took inspiration from{" "}
                    <Anchor
                      target="_blank"
                      href="https://www.youtube.com/watch?v=G_UYXzGuqvM"
                    >
                      a sudoku solving algorithm
                    </Anchor>
                    . This algorithm attempts coloring regions until it is
                    unable to, in which case it backtracks and tries a different
                    combination of colors.
                  </Text>
                </td>
              </tr>
            </tbody>
          </Table>
        </section>

        <div id="tech"></div>
        <Space h="md" />
        <Divider my="lg" />
        <Space h="md" />

        <section>
          <Title order={2}>Technology</Title>

          <Text>
            The main application is a{" "}
            <Anchor target="_blank" href="https://unity.com/">
              Unity
            </Anchor>{" "}
            application built for{" "}
            <Anchor
              target="_blank"
              href="https://developer.mozilla.org/en-US/docs/Web/API/WebGL_API"
            >
              WebGL
            </Anchor>
            . I did not import any external Unity packages. The website is a
            static page written using the following technologies:
          </Text>

          <List>
            <List.Item>
              <Anchor target="_blank" href="https://reactjs.org/">
                React
              </Anchor>
            </List.Item>
            <List.Item>
              <Anchor target="_blank" href="https://www.typescriptlang.org/">
                TypeScript
              </Anchor>
            </List.Item>
            <List.Item>
              <Anchor target="_blank" href="https://mantine.dev/">
                Mantine
              </Anchor>
            </List.Item>
            <List.Item>
              <Anchor target="_blank" href="https://pages.github.com/">
                GitHub Pages
              </Anchor>
            </List.Item>
          </List>
        </section>

        <div id="next"></div>
        <Space h="md" />
        <Divider my="lg" />
        <Space h="md" />

        <section id="#next">
          <Title order={2}>What's Next?</Title>

          <Text>
            I currently have no future plans for this project, but all of the
            code is available in a{" "}
            <Anchor
              target="_blank"
              href="https://github.com/technologeli/map-colorizer"
            >
              GitHub repository
            </Anchor>
            , if you would like to fork the project or make a pull request.
          </Text>
        </section>

        <Space h="xl" />
        <Space h="xl" />
      </Container>
    </AppShell>
  );
};

export default Page;
